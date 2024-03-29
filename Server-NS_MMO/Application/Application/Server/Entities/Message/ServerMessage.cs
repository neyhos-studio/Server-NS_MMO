﻿using System;

namespace Application.Server.Entities.Message
{
    class SendMessage
    {
        private const string ID = "SERVER";
        public string action { get; set; }
        public string[] data { get; set; }


        public SendMessage() { }


        public SendMessage(string action, string[] data)
        {
            this.action = action;
            this.data = data;
        }

        public string getMessage()
        {
            string datasToString = String.Empty;
            int i = 1;
            foreach (string d in this.data)
            {
                datasToString += d;

                if (i != this.data.Length)
                {
                    datasToString += ";";
                }
                i++;
            }
            return String.Format("{0}|{1}|{2}", ID, this.action, datasToString);
        }
    }
}
