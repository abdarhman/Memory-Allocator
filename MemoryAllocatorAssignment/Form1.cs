using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryAllocatorAssignment
{
    public partial class Form1 : Form
    {
        LinkedList<MemoryElement> Memory = new LinkedList<MemoryElement> ();
        LinkedList<MemoryElement> save = new LinkedList<MemoryElement> ();
        LinkedList<MemoryElement> Processes = new LinkedList<MemoryElement>();
        int pi = 0;
        int hi = 0;
        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = "Choose from here !";
            comboBox1.Items.Add("Best Fit");
            comboBox1.Items.Add("First Fit");
            comboBox1.Items.Add("Worst Fit");
            label5.Text = "Hole"+hi.ToString();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MemoryElement temp = new MemoryElement("Hole"+hi.ToString(), "Hole", Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
            Memory.AddLast(temp);
            MemoryElement savetemp = new MemoryElement("Hole" + hi.ToString(), "Hole", Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
            save.AddLast(savetemp);
            hi++;
            label5.Text = "Hole" + hi.ToString();
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MemoryElement temp = new MemoryElement("Process" + pi.ToString(), "Process",0, Convert.ToInt32(textBox3.Text));
            Processes.AddLast(temp);
            pi++;
            textBox3.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            label5.Visible = false;
        }
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            textBox4.Text = "";
            IEnumerable<MemoryElement> SortedMemory;
            if (comboBox1.Text == "Best Fit") { SortedMemory =  Memory.OrderBy( memoryElement =>memoryElement.size); textBox4.AppendText("You have chosen Best Fit option" + Environment.NewLine); }
            else if (comboBox1.Text == "First Fit") { SortedMemory = Memory.OrderBy(memoryElement => memoryElement.Base); textBox4.AppendText("You have chosen First Fit option !" + Environment.NewLine); }
            else { SortedMemory = Memory.OrderByDescending(memoryElement => memoryElement.size); textBox4.AppendText("You have chosen Worst Fit option !" + Environment.NewLine); }
            List<MemoryElement> SortedMemoryList = SortedMemory.ToList();
            //for (int k = 0; k < Processes.Count; k++)
            foreach (MemoryElement pros in Processes)
            {
                //for (int i = 0; i < Memory.Count; i++)
                foreach (MemoryElement me in SortedMemoryList)
                {
                    if (me.type == "Hole")
                    {
                        if (pros.size <= me.size)
                        {
                            textBox4.AppendText(pros.Name + " would be allocated at " + me.Name + Environment.NewLine);
                            me.size = me.size - pros.size;
                            textBox4.AppendText(me.Name + "'s new size = " + me.size.ToString() + Environment.NewLine);
                            pros.Base = me.Base;
                            textBox4.AppendText(pros.Name + "'s allocated base = " + pros.Base.ToString() + Environment.NewLine);
                            me.Base = me.Base + pros.size;
                            textBox4.AppendText(me.Name + "'s new base = " + me.Base.ToString() + Environment.NewLine);
                            if (me.size == 0)
                            { Memory.Remove(me); textBox4.AppendText(me.Name + " is deleted !" + Environment.NewLine); }
                            //Memory.Add(Processes[k]);
                            SortedMemoryList.Add(pros);
                            break;
                        }
                    }
                }
            }
            //Memory.Sort(CompareBases);
            IEnumerable<MemoryElement> RenderedList = SortedMemoryList.OrderBy(memoryElement => memoryElement.Base);
            //for (int i = 0; i < Memory.Count; i++)
            foreach (MemoryElement fu in RenderedList)
            {
                ListViewItem lvi = new ListViewItem(fu.Name);
                lvi.SubItems.Add(fu.Base.ToString());
                lvi.SubItems.Add(fu.size.ToString());
                lvi.SubItems.Add((fu.Base + fu.size).ToString());
                listView1.Items.Add(lvi);
                if (fu.type == "Hole")
                {
                    ListViewItem free = new ListViewItem(fu.Name);
                    free.SubItems.Add(fu.Base.ToString());
                    free.SubItems.Add((fu.Base + fu.size).ToString());
                    listView2.Items.Add(free);
                }
                else if (fu.type == "Process")
                {
                    ListViewItem occupied = new ListViewItem(fu.Name);
                    occupied.SubItems.Add(fu.Base.ToString());
                    occupied.SubItems.Add((fu.Base + fu.size).ToString());
                    listView3.Items.Add(occupied);
                }

            }
            Memory.Clear();
            foreach (MemoryElement u in save)
            {
                Memory.AddLast(new MemoryElement(u.Name, u.type, u.Base, u.size));
            }
            MessageBox.Show("Go to Review Steps tab for step by step details !");
        }
        private int CompareBases (MemoryElement m1, MemoryElement m2)
        {
            return m1.Base.CompareTo(m2.Base);
            //if (m1.Base >= m2.Base) { return 1; }
            //else { return 0; }
        }
        private int CompareSizes (MemoryElement m1, MemoryElement m2)
        {
            if (m1.size >= m2.size) { return 1; }
            else { return 0; }
        }
        private int InverseCompareSizes(MemoryElement m1, MemoryElement m2)
        {
            if (m1.size >= m2.size) { return 0; }
            else { return 1; }
        }

        
    }
    public class MemoryElement
    {
        public string Name,type;
        public int Base, size;
        public MemoryElement(string n, string t, int b, int s)
        { Name = n; type = t; Base = b; size = s; }
    }
}
