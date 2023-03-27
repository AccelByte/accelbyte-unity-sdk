// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
#include <signal.h>

void (*sig_term_action)(int);

void sig_callback(int signal_code){
	sig_term_action(signal_code);
}

void on_term(void (*handler)(int)){
	sig_term_action = handler;			
	signal(SIGTERM, sig_callback);
}