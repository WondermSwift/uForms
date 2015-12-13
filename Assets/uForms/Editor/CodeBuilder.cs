﻿using System.Text;

namespace uForms
{
    public class CodeBuilder
    {
        int indent = 0;
        StringBuilder builder = new StringBuilder();

        public int Indent
        {
            get
            {
                return this.indent;
            }
            set
            {
                this.indent = value;
            }
        }
        
        public void WriteLine(string line)
        {
            this.AddIndent();
            this.builder.Append(line + "\r\n");

        }

        private void AddIndent()
        {
            for(int i = 0; i < this.indent; ++i)
            {
                this.builder.Append("    ");
            }
        }

        public string GetCode()
        {
            return this.builder.ToString();
        }
    }
}