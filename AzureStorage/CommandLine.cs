using System.Collections.Generic;
using System;

namespace Utils
{
    class CommandLine
    {
        string _command = "";

        string _path = "";
        List<string> _values = new List<string>();
        string _container = "";

        int _pos = 0;
        public string command
        {
            get { return _command;}
        }

        public string path
        {
            get { return _path;}
        }

        public string container
        {
            get {return _container;}
        }

        public List<string> args 
        {
            get { return _values; }
        }

        void PrintHelper()
        {
        }

        public bool Parser(string[] cmd)
        {
            bool ret = true;
            while (cmd.Length > _pos && ret == true)
            {
                switch (cmd[_pos])
                {
                    case "-c":
                    case "--container":
                        {
                            _pos++;
                            if (cmd.Length > _pos && cmd[_pos][0] != '-')
                            {
                                _container = cmd[_pos];
                                _pos++;
                            }
                            else
                            {
                                ret = false;
                            }
                            break;
                        }
                    case "-l":
                    case "--list":
                        {    
                            _pos++;
                            if(cmd.Length > _pos && _command == "" && (cmd[_pos] == "containers" || cmd[_pos] == "blobs"))
                            {
                                _command = "list";
                                _values.Add(cmd[_pos]);
                                _pos++;
                            }
                            else
                            {
                                ret = false;
                            }
                            break;
                        }
                    case "-u":
                    case "--upload":
                        {
                            if (_command == "")
                            {
                                _pos++;
                                _command = "upload";
                                while (cmd.Length > _pos && cmd[_pos][0] != '-')
                                {
                                    _values.Add(cmd[_pos]);
                                    _pos++;
                                }
                                if (_values.Count == 0)
                                {
                                    ret = false;
                                }
                            }
                            else
                            {
                                ret = false;
                            }
                            break;
                        }
                    case "-d":
                    case "--download":
                        {
                            if(_command == "")
                            {
                                _pos++;
                                _command = "download";

                                List<string> v = new List<string>();
                                while (cmd.Length > _pos && cmd[_pos][0] != '-')
                                {
                                    _values.Add(cmd[_pos]);
                                    _pos++;
                                }
                                if (_values.Count == 0)
                                {
                                    ret = false;
                                }
                            }
                            else
                            {
                                ret = false;
                            }
                            break;
                        }
                    case "-p":
                    case "--path":
                        {
                            _pos++;
                            if (cmd.Length > _pos && cmd[_pos][0] != '-')
                            {
                                _path = cmd[_pos];
                                _pos++;
                            }
                            else
                            {
                                ret = false;
                            }
                            break;
                        }
                }
            }
            return ret;
        }
    }
}
