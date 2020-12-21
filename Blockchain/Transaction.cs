namespace Blockchain
{
    public class Transaction
    {
        public byte[] RandomBytes { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public decimal Value { get; set; }
        public int ToStorage { get; set; }
        public byte[] CurrentHash { get; set; }
        public byte[] Signature { get; set; }
    }
}
