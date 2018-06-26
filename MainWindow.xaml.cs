using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace LachesisStats
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseConnection db;

        public MainWindow()
        {
            InitializeComponent();

            db = new DatabaseConnection("sa", "verbatim", "172.16.105.70", "DeskStats");
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            LacStats stats = GetStats();

            var statCountPerControlID = from s in stats
                                        group s by s.ControlId into g
                                        select new
                                        {
                                            id = g.Key,
                                            sum = g.Sum(x => x.StatCount)
                                        };
                                        

            foreach(var s in statCountPerControlID)
            {
                // add to new collection
                txtblk_main.Text += $"{s.id}|{s.sum} \r\n";
            }
        }

        private LacStats GetStats() // Date range as arguments?
        {
            DataSet ds = db.ExecuteNonScalarQuery(@"select * from stats where time >= '2018-05-08 12:00:00' order by control_id asc");
            DataTable dt = ds.Tables["TABLE"];
            LacStats stats = new LacStats();


            foreach (DataRow row in dt.Rows)
            {
                LacStat stat = new LacStat(Convert.ToInt32(row["control_id"]), Convert.ToInt32(row["statcount"]));
                stats.Add(stat);
            }

            return stats;
        }
    }

    public class LacStat
    {
        public int StatCount = 0;
        public int ControlId { get; set; }

        public LacStat(int controlid, int statcount)
        {
            StatCount = statcount;
            ControlId = controlid;
        }
    }

    public class LacStats : List<LacStat>
    {

    }

}
