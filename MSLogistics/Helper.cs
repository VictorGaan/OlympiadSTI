using System.Windows.Controls;
namespace MSLogistics
{
    public class Helper
    {
        public static Blockchain.Blockchain Blockchain = new Blockchain.Blockchain();
        public static MSLogisticsContext context = new MSLogisticsContext();
        public static Frame MainFrame;
        public static int client = 0;
        public static int employee = 0;
    }
}
