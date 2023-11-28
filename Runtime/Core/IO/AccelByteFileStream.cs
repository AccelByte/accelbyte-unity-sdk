// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace AccelByte.Core
{
    public class AccelByteFileStream
    {
        internal Action OnPop;
        List<Action> ioActions;

        public AccelByteFileStream()
        {
            ioActions = new List<Action>();
        }

        public void Pop()
        {
            lock (ioActions)
            {
                if (ioActions.Count == 0)
                {
                    return;
                }

                foreach (Action action in ioActions)
                {
                    action.Invoke();
                }
                ioActions.Clear();
                OnPop?.Invoke();
            }
        }

        public void WriteFile(IFormatter formatter, object content, string path, System.Action<bool> onDone, bool instantWrite = false)
        {
            Action writeAction = () =>
            {
                string pathDirectory = Path.GetDirectoryName(path);
                if(!Directory.Exists(pathDirectory))
                {
                    Directory.CreateDirectory(pathDirectory);
                }

                try
                {
                    using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(stream, content);
                    }
                    onDone?.Invoke(true);
                }
                catch(Exception ex)
                {
                    AccelByteDebug.LogVerbose($"Write file failure.\n{ex.Message}");
                    onDone?.Invoke(false);
                }
            };

            if (!instantWrite)
            {
                lock (ioActions)
                {
                    ioActions.Add(writeAction);
                }
            }
            else
            {
                writeAction();
            }
        }

        public void ReadFile(IFormatter formatter, string path, System.Action<bool, object> onDone)
        {
            Action readAction = () =>
            {
                if(!File.Exists(path))
                {
                    onDone?.Invoke(false, null);
                    return;
                }

                try
                {
                    object result = null;
                    using (FileStream stream = new FileStream(path, FileMode.Open))
                    {
                        result = formatter.Deserialize(stream);
                    }
                    onDone?.Invoke(true, result);
                }
                catch (Exception ex)
                {
                    AccelByteDebug.LogVerbose($"Read file failure.\n{ex.Message}");
                    onDone?.Invoke(false, null);
                }
            };

            lock (ioActions)
            {
                ioActions.Add(readAction);
            }
        }

        public void DeleteFile(string path, System.Action<bool> onDone)
        {
            Action readAction = () =>
            {
                if (!File.Exists(path))
                {
                    onDone?.Invoke(false);
                    return;
                }

                try
                {
                    File.Delete(path);
                    onDone?.Invoke(true);
                }
                catch (Exception ex)
                {
                    AccelByteDebug.LogVerbose($"Delete file failure.\n{ex.Message}");
                    onDone?.Invoke(false);
                }
            };

            lock (ioActions)
            {
                ioActions.Add(readAction);
            }
        }
    }
}
