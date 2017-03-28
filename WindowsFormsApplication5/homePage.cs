using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;
using System.Threading;
using System.Diagnostics;

namespace collegeModules
{
    public partial class homePage : Form
    {



        public static string moduleValue;

        public static string myAssignmentListBoxValue;

        public homePage()
        {
            InitializeComponent();

            try

            {

                dynamic dir = @"Resources\Modules";//Path to load in modules from 

                foreach (string file in Directory.GetFiles(dir))
                {
                    string fileNoExt = "";


                    if (file.Length > 5)
                    {
                         fileNoExt = Path.GetFileNameWithoutExtension(file);//Loads in avaible files to comboBox
                    }


                    if (fileNoExt.Length > 0)
                        this.comboBox1.Items.Add(fileNoExt);
                }


            }//End of try

            catch
            {
                MessageBox.Show("Cannnot read in available modules!"); //Catches error if modules can;t be loaded 
            }

        }



        private void button1_Click(object sender, EventArgs e)//Add module button
        {


            var selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);

            moduleValue = selected;


            if (tabControl1.TabPages.ContainsKey(selected))//Checks if selected module has already been added.

            {

                MessageBox.Show("Module already added to Application, please choose another.", "ERROR");
                //Tells the user if they have already added a module to the application.
            }

            else
            {

                listBox1.Items.Add(selected);//Adds selected module to module list view

                //*********************LISTBOX1 SORTING

                List<string> moduleList = new List<string>();//New instance of a list

                foreach (string module in listBox1.Items)//for every item in listbox1

                {
                    moduleList.Add(module);//adds each module in listbox to list(moduleList)
                }

                string[] modules = moduleList.ToArray();//Converts list(moduleList) to an array(modules)

                //*******//Start of timing for the module list sorting

                //Stopwatch stopwatch = new Stopwatch();
                // stopwatch.Start();

                sort s = new sort();

               

                s.InsertionSort(modules);//label4.Text = "Insertion Sort Method Timings for Module List ";//starts the InsertionSort method with the module array being passed

                // Quicksort(modules, 0, modules.Length - 1); label4.Text = "Quick Sort Method Timings for Module List";//starts the QuickSort sort method with the module array being passed

                //stopwatch.Stop();

                //listBox3.Items.Add((listBox3.Items.Count+1) + " Assignments in list to sort: " + stopwatch.Elapsed.ToString());

                //********//End of timing for the module list sorting 

                listBox1.Items.Clear();//Clears the old list

                listBox1.Items.AddRange(modules);//adds in the new sorted list

                //*********************

                TabPage tb = new TabPage(selected); //create tab
                tabControl1.TabPages.Add(tb); // add tab to existed TabControl

                tb.Name = selected;

                moduleTab myUserControl = new moduleTab();  //  Creates an instance of the user controls class
                myUserControl.Dock = DockStyle.Fill;
                tb.Controls.Add(myUserControl);  // adds user controls design to newly created tab




                try
                {
                    ////////////////Look for dates///////

                    foreach (string eachAssignment in moduleTab.MyAssignments)


                    {


                        listBox2.Items.Add(eachAssignment);



                    }//end of foreach




                    //*********************LISTBOX2 SORTING (Assignments)


                    List<string> moduleList2 = new List<string>();//New instance of a list

                    foreach (string module in listBox2.Items)//for every item module in listbox1

                    {
                        moduleList2.Add(module);//adds each module in listbox to list(moduleList)


                    }


                    string[] modules2 = moduleList2.ToArray();//Converts list(moduleList) to an array(modules)

                    //********//Start of the timing for the assignment sorting

                    //Stopwatch stopwatch = new Stopwatch();
                    //stopwatch.Start();

                    s.InsertionSort(modules2); //label4.Text = "Insertion Sort Method Timings for Assignment List ";//starts the InsertionSort method with the module array being passed

                    //Quicksort(modules2, 0, modules2.Length - 1); label4.Text = "Quick Sort Method Timings for Assignment List";//starts the QuickSort sort method with the module array being passed

                    //stopwatch.Stop();

                    //listBox3.Items.Add((listBox3.Items.Count + 1) + " Assignments in list to sort: " + stopwatch.Elapsed.ToString());

                    //********//End of the timing 


                    
                    listBox2.Items.Clear();//Clears the old listBox2
                    listBox2.Items.AddRange(modules2);//adds in the new sorted list

                    //*********************

                }//End of try 

                catch

                {
                    //Does nothing if there are no Assignments in the 
                }

            }//End of else

        }//End of method


        private void button2_Click(object sender, EventArgs e)//Remove Button
        {

            string selectedModule = "";

            //Following code removes module tab from tab control 
            foreach (Object selecteditem in listBox1.SelectedItems)
            {

                string tabToRemove = selecteditem as string; //Tab to remove is selected module in listBox 

                selectedModule = selecteditem as string;

                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    if (tabControl1.TabPages[i].Name.Equals(tabToRemove, StringComparison.OrdinalIgnoreCase))
                    {
                        tabControl1.TabPages.RemoveAt(i);

                    }
                }

            }



            //Removes module from listbox 1
            for (int i = listBox1.SelectedIndices.Count - 1; i >= 0; i--)
            {

                listBox1.Items.RemoveAt(listBox1.SelectedIndices[i]);


            }

            //Removes all Assignments along with the Removed module.
            for (int n = listBox2.Items.Count - 1; n >= 0; --n)
            {
                string removelistitem = selectedModule;
                if (listBox2.Items[n].ToString().Contains(removelistitem))
                {
                    listBox2.Items.RemoveAt(n);
                }
            }


        }//End of method

        

        private void button3_Click(object sender, EventArgs e)//Assignment Notes Editor Button
        {
            var selected = listBox2.GetItemText(listBox2.SelectedItem);

            myAssignmentListBoxValue = selected;

            if (selected == "")
                MessageBox.Show("Please select an Assignment!");//If an assignment hasn't be selected program asks user to select one

            else
            {
                assignmentNotes assNotes = new assignmentNotes();//opens assignment notes pane
                assNotes.Show();
            }


        }




    }



    class sort //Sepreate class for sorting methods
    {
        public void InsertionSort(IComparable[] array)
        {
            int a, b;

            for (a = 1; a < array.Length; a++)
            {
                IComparable value = array[a]; //Element to be inserted

                b = a - 1;//Postion on the right sideof the sorted sub array

                while ((b >= 0) && (array[b].CompareTo(value) > 0)) //While not in position
                {
                    array[b + 1] = array[b];//Moves to the right
                    b--; //Continue
                }

                array[b + 1] = value;
            }
        }

        public static void Quicksort(IComparable[] elements, int left, int right)
        {
            int a = left, b = right;
            IComparable pivot = elements[(left + right) / 2];

            while (a <= b)
            {
                while (elements[a].CompareTo(pivot) < 0)
                {
                    a++;
                }

                while (elements[b].CompareTo(pivot) > 0)
                {
                    b--;
                }

                if (a <= b)
                {
                    // Swap
                    IComparable tmp = elements[a];
                    elements[a] = elements[b];
                    elements[b] = tmp;

                    a++;
                    b--;
                }
            }

            // Recursive calls
            if (left < b)
            {
                Quicksort(elements, left, b);
            }

            if (a < right)
            {
                Quicksort(elements, a, right);
            }
        }

       
    }

}

