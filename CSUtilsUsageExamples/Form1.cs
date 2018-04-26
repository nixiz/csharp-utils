using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSUtils;

namespace CSUtilsUsageExamples
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MakeAll<CheckBox>(cb => cb.Checked = true);
            foreach (RadioButton rb in this.GetAll<RadioButton>().Where(r => r.Tag.Parse<int>() == 0))
            {
                rb.Checked = true;
            }
        }

        private void bt_shift_names_basic_Click(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            string[] buttonTextArray = lbl_shift_names_basic.Text.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            bt.Enumerate(buttonTextArray, (s, newTag) => {
                Console.WriteLine("new tag value is {0}", newTag.Parse<int>());
            });
        }

        private Dictionary<string, object> namesWithColors = new Dictionary<string, object>()
        {
            { "Red" , Color.MediumVioletRed },
            { "Yellow" , Color.Yellow },
            { "Green" , Color.LightGreen },
        };

        private void bt_shift_names_with_colors_Click(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            bt.Enumerate(namesWithColors, (s, o) => {
                Color newColor = (Color)o;
                bt.BackColor = newColor;
                Console.WriteLine("new color is {0}", newColor.Name);
            });
        }

        private void rb_txt_bold_CheckedChanged(object sender, EventArgs e)
        {
            Console.WriteLine("RB: {0}  Checked {1}", (sender as RadioButton).Text, (sender as RadioButton).Checked);
            RadioButton rb = sender as RadioButton;
            if (!rb.Checked) return;

            FontStyle newFontStyle = (FontStyle)rb.Tag.Parse<int>();

            this.MakeAll<Label>(c => {
                c.Font = new Font(rb.Font.Name, rb.Font.Size, newFontStyle);
            });
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            tb_num_of_chked_boxes.Text = this.GetAll<CheckBox>().Where(cb => cb.Checked == true).Count().ToString();
        }

        private void bt_link_unlink_Click(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            bt.Enumerate(new string[] { "Link", "Unlink" }, (c, o) => {
                if (c.Text == "Link")
                {
                    UnLinkGroupBoxes();
                }
                else
                {
                    LinkGroupBoxes();
                }
            });
        }

        private void LinkGroupBoxes()
        {
            rb1_master_1.Link("Checked", rb2_master_1);
            rb1_master_1.LinkOneWay("Checked", rb3_slave_1, rb4_slave_1);

            rb1_master_2.Link("Checked", rb2_master_2);
            rb1_master_2.LinkOneWay("Checked", rb3_slave_2, rb4_slave_2);

            rb1_master_3.Link("Checked", rb2_master_3);
            rb1_master_3.LinkOneWay("Checked", rb3_slave_3, rb4_slave_3);
        }

        private void UnLinkGroupBoxes()
        {
            rb1_master_1.UnLink();
            rb1_master_2.UnLink();
            rb1_master_3.UnLink();
        }
    }
}
