// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

//error codes
//0 none
//1 WebSocket object not found
//2 Invalid websocket state
//3 Invalid code

var WebsocketBridge = {
	$wsStore: {
		objects: [],
		lastObjectId: 0,
		
		//event handlers
		onOpen: null,
		onMessage: null,
		onError: null,
		onClose: null,
	},

	WsSetEventHandlers: function(onOpen, onMessage, onError, onClose) {
		wsStore.onOpen = onOpen;
		wsStore.onMessage = onMessage;
		wsStore.onError = onError;
		wsStore.onClose = onClose;
	},

	WsCreate: function(url, protocols) {
		var urlStr = Pointer_stringify(url);
		var protocolsStr = Pointer_stringify(protocols);
		var objectId = wsStore.lastObjectId++;
		wsStore.objects[objectId] = { url : urlStr, protocols : protocolsStr };

		return objectId;
	},

	WsDestroy: function(objectId) {
	    console.log("CLOSING WEB SOCKET");
	
		if (!wsStore.objects[objectId]) return 1;

		var ws = wsStore.objects[objectId].ws;

		if (!ws) return 0;

		// Close if not closed
		if (ws !== null && ws.readyState < 2)
			ws.close();

		// Remove reference
		delete wsStore.objects[objectId];
		
		return 0;
	},

	WsOpen: function(objectId) {
		var object =  wsStore.objects[objectId];

		if (!object) return 1;

		object.ws = new WebSocket(object.url, object.protocols);
		var ws = object.ws;
		ws.binaryType = 'arraybuffer';

		console.log(ws);

		ws.onopen = function() {
            if (wsStore.onOpen === null) return;
            try{
				Runtime.dynCall('vi', wsStore.onOpen, [objectId]);
			}catch(e){
				console.log(e);			
			}
		};

        ws.onmessage = function(ev) {
            if (wsStore.onMessage === null) return;
            
            var msgLength = lengthBytesUTF8(ev.data) + 1;
            var msgBuffer = _malloc(msgLength);
            stringToUTF8(ev.data, msgBuffer, msgLength);

            try {
                Runtime.dynCall('vii', wsStore.onMessage, [objectId, msgBuffer])
            } finally {
                _free(msgBuffer);
            }
        };

        ws.onerror = function(ev) {
            if (wsStore.onError === null) return;
                            
            var msg = "WebSocket error: " + ev.data;
            var msgLength = lengthBytesUTF8(msg) + 1;
            var msgBuffer = _malloc(msgLength);
            stringToUTF8(msg, msgBuffer, msgLength);

            try {
                Runtime.dynCall('vii', wsStore.onError, [objectId, msgBuffer]);
            } finally {
                _free(msgBuffer);
            }
        };

        ws.onclose = function(ev) {
            if (wsStore.onClose) {
				try{
					Runtime.dynCall('vii', wsStore.onClose, [ objectId, ev.code ]);
				}catch(e){
					console.log(e);
				}
			}
        };
		return 0;
	},

	WsClose: function(objectId, code, reasonPtr) {
		if (!wsStore.objects[objectId]) return 1;

		var ws = wsStore.objects[objectId].ws;
        wsStore.objects[objectId].ws = null;
		
		if (!ws) return 0;

		ws.onopen = null;
		ws.onmessage = null;
		ws.onerror = null;
		ws.onclose = null;		

		if (ws.readyState === 2 || ws.readyState === 3)	return 0;

		var reason = ( reasonPtr ? Pointer_stringify(reasonPtr) : undefined );
		
		try {
		    if (reason) ws.close(code, reason);
		    else ws.close(code);
		} catch(err) {
			return 3;
		}

		return 0;
	},

	WsSend: function(objectId, message) {
		if (!wsStore.objects[objectId]) return 1;

		var ws = wsStore.objects[objectId].ws;
		
		if (!ws) return 1;

		if (ws.readyState !== 1) return 2;

	    var message = Pointer_stringify(message);
		ws.send(message);

		return 0;
	},

	WsGetReadyState: function(objectId) {
		if (!wsStore.objects[objectId]) return 1;

		var ws = wsStore.objects[objectId].ws;
	
		if (!ws) return 1;

		return ws.readyState;
	}

};

autoAddDeps(WebsocketBridge, '$wsStore');
mergeInto(LibraryManager.library, WebsocketBridge);
