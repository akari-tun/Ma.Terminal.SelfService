using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Ma.Terminal.SelfService.Resource
{
    public class ResourceManager
    {
        static ResourceManager _resourceHelper;
        ResourceDictionary _stringResourceDictionary;

        public static ResourceManager Instance
        {
            get
            {
                if (_resourceHelper == null)
                {
                    _resourceHelper = new ResourceManager();
                }

                return _resourceHelper;
            }
        }

        public ResourceDictionary StringResourceDictionary
        {
            get => _stringResourceDictionary;
            set
            {
                _stringResourceDictionary = value;
            }
        }

        public string GetString(string key)
        {
            return _stringResourceDictionary.Contains(key) ? _stringResourceDictionary[key].ToString() : string.Empty;
        }
    }
}
