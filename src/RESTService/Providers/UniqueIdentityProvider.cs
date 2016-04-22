﻿using System.Runtime.Serialization;

namespace RESTService.Providers
{
    /// <summary>
    /// Provides unique id number 
    /// </summary>
    [DataContract]
    public class UniqueIdentityProvider : IIdentityProvider<int>
    {
        [DataMember]
        private int _lastId = 1;

        public int Id => _lastId++;
    }
}