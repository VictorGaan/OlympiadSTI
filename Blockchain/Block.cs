using System;
using System.Collections.Generic;

namespace Blockchain
{
    public class Block:ICloneable
    {
        public int Nonce { get; set; }
        public int Difficulty { get; set; }
        public byte[] CurrentHash { get; set; }
        public byte[] PreviousHash { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string Miner { get; set; }
        public string Data { get; set; }
        public byte[] Signature { get; set; }
        public DateTime TimeStamp { get; set; }
        public Dictionary<string, decimal> Mapping { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
