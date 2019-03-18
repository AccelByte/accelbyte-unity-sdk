// Copyright (c) 2018-2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.IO;
using System.Text;

namespace AccelByte.Api
{
    public class FormDataContent
    {
        private string boundary;
        private MemoryStream stream;
        private StreamWriter writer;

        public FormDataContent()
        {
            this.boundary = "-----------" + Guid.NewGuid().ToString().Replace("-", "");
            this.stream = new MemoryStream();
            this.writer = new StreamWriter(this.stream, Encoding.ASCII);
            this.writer.Write("--{0}", this.boundary);
        }

        public FormDataContent Add(byte[] data, string filename)
        {
            this.writer.Write(
                "\r\nContent-Disposition: form-data; name=\"file\"; filename=\"{0}\"\r\nContent-Type: {1}\r\n\r\n",
                filename,
                "octet/stream");

            this.writer.Flush();

            this.writer.BaseStream.Write(data, 0, data.Length);
            this.writer.Write("\r\n--{0}", this.boundary);

            return this;
        }

        public FormDataContent Add(string name, string value)
        {
            this.writer.Write("\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", name, value);

            this.writer.Write("\r\n--{0}", this.boundary);

            this.writer.Flush();

            return this;
        }

        public string GetMediaType() { return "multipart/form-data; boundary=" + this.boundary; }

        public byte[] Get()
        {
            this.writer.Write("--\r\n");
            this.writer.Flush();

            return this.stream.ToArray();
        }
    }
}