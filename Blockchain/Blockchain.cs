using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain
{
    public class Blockchain
    {
        const int Difficulty = 20;
        const int RandBytes = 32;
        const int StartPercent = 10;
        const int StorageReward = 1;
        private int Storage = 100;
        public List<Block> Chain { get; set; }
        public Blockchain(string receiver)
        {
            Helper.InitFileSystem();
            Chain = new List<Block>();
            CreateGenesisBlock(receiver);
        }
        public Blockchain()
        {

        }

        private void CreateGenesisBlock(string receiver)
        {
            Block block = new Block()
            {
                PreviousHash = null,
                Mapping = new Dictionary<string, decimal>(),
                Miner = receiver,
                TimeStamp = DateTime.UtcNow
            };
            block.Mapping.Add("StorageChain", 100);
            block.Mapping.Add($"{receiver}", 100);
            block.CurrentHash = BlockHash(block);
            AddBlock(block);
        }
        private byte[] BlockHash(Block block)
        {
            string result = "";
            result += block.Nonce;
            result += block.Difficulty;
            result += block.PreviousHash;
            result += block.Transactions;
            result += block.Mapping;
            result += block.Miner;
            result += block.TimeStamp;
            return SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(result));
        }
        public Block CreateNewBlock(Database.Client miner, Database.Order order)
        {
            Block block = new Block()
            {
                Nonce = new Random().Next(1, 9999),
                PreviousHash = GetLatestBlock().CurrentHash,
                Mapping = new Dictionary<string, decimal>(),
                Miner = miner.PublicKey,
                Data = ObjectToString(order),
                Transactions = new List<Transaction>(),
                Difficulty = Difficulty,
                TimeStamp = DateTime.UtcNow,
            };
            block.CurrentHash = BlockHash(block);
            if (Mine(block))
            {
                block.Mapping.Add("StorageChain", Storage);
                block.Mapping.Add($"{miner.PublicKey}", miner.Wallet);
                block.Signature = SignBlock(block, miner);
                AddBlock(block);
                return block;
            }
            else return null;
        }

        private void AddBlock(Block block)
        {
            Chain.Add(block);
        }
        private Block GetLatestBlock()
        {
            return Chain[^1];
        }

        public void AddTransaction(Database.Client from, Database.OrganizationAccount to, Database.Order order, decimal value)
        {
            if (Chain.Count == 1)
            {
                Block newBlock = CreateNewBlock(from, order);
                newBlock.Transactions.Add(NewTransaction(from, to, value));
            }
            else
            {
                Block last = GetLatestBlock();
                if (last.Transactions.Count < 1)
                {
                    last.Transactions.Add(NewTransaction(from, to, value));
                }
                else
                {
                    Block newBlock = CreateNewBlock(from, order);
                    newBlock.Transactions.Add(NewTransaction(from, to, value));
                }
            }

        }
        public void AddReturnTransaction(Database.OrganizationAccount from, Database.Client to, Database.Order order, decimal value)
        {
            if (Chain.Count == 1)
            {
                Block newBlock = CreateNewBlock(to, order);
                newBlock.Transactions.Add(ReturnTransaction(from, to, value));
            }
            else
            {
                Block last = GetLatestBlock();
                if (last.Transactions.Count < 1)
                {
                    last.Transactions.Add(ReturnTransaction(from, to, value));
                }
                else
                {
                    Block newBlock = CreateNewBlock(to, order);
                    newBlock.Transactions.Add(ReturnTransaction(from, to, value));
                }
            }

        }
        public bool Mine(Block block)
        {

            Block clone = (Block)block.Clone();
            clone.Nonce = 0;
            clone.CurrentHash = null;
            while (clone.CurrentHash == null || !clone.CurrentHash.SequenceEqual(block.CurrentHash))
            {
                clone.Nonce++;
                clone.CurrentHash = BlockHash(clone);
            }
            return clone.CurrentHash.SequenceEqual(block.CurrentHash);
        }
        private Transaction NewTransaction(Database.Client from, Database.OrganizationAccount to, decimal value)
        {
            Transaction transaction = new Transaction()
            {
                RandomBytes = GenerateRandomByte(RandBytes),
                Sender = from.PublicKey,
                Receiver = to.PublicKey,
                Value = value,
            };

            if (value > StartPercent)
            {
                transaction.ToStorage = StorageReward;
                Storage += StorageReward;
                value -= StorageReward;
            }
            from.Wallet -= value;
            to.Wallet += value;
            transaction.CurrentHash = TransactionHash(transaction);
            transaction.Signature = SignTransaction(transaction.CurrentHash, from);
            return transaction;
        }
        private Transaction ReturnTransaction(Database.OrganizationAccount from, Database.Client to, decimal value)
        {
            Transaction transaction = new Transaction()
            {
                RandomBytes = GenerateRandomByte(RandBytes),
                Sender = from.PublicKey,
                Receiver = to.PublicKey,
                Value = value,
            };
            if (value > StartPercent)
            {
                transaction.ToStorage = StorageReward;
                Storage += StorageReward;
                value -= StorageReward;
            }
            from.Wallet -= value;
            to.Wallet += value;
            transaction.CurrentHash = TransactionHash(transaction);
            transaction.Signature = SignTransaction(transaction.CurrentHash, to);
            return transaction;
        }
        private byte[] GenerateRandomByte(int length)
        {
            Random rand = new Random();
            byte[] buffer = new byte[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = (byte)rand.Next(256);
            }
            return buffer;
        }
        private byte[] SignTransaction(byte[] transaction, Database.Client miner)
        {
            return transaction.Concat(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(miner.PrivateKey))).ToArray();
        }
        private byte[] SignBlock(Block block, Database.Client miner)
        {
            return BlockHash(block).Concat(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(miner.PrivateKey))).ToArray();
        }
        private byte[] TransactionHash(Transaction transaction)
        {
            string result = "";
            result += transaction.RandomBytes;
            result += transaction.Sender;
            result += transaction.Receiver;
            result += transaction.Value;
            result += transaction.ToStorage;
            return SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(result));
        }
        public string ObjectToString(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            byte[] bytes = Encoding.Default.GetBytes(json);
            return Convert.ToBase64String(bytes);
        }

        public Database.Order StringToObject(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            string json = Encoding.Default.GetString(bytes);
            return JsonConvert.DeserializeObject<Database.Order>(json);
        }
    }
}
