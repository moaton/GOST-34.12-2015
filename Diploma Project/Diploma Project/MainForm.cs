using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diploma_Project
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Панельдер, батырмалар, өрістер бойынша шолу
        /// --------------------------
        /// panel1 - бүйір мәзір панелі
        /// {
        ///     label1 - ГОСТ 34-12.2015сөзі жапсырмасы
        ///     label2 - шифрлеу сөзі жапсырмасы
        ///     label3 - дешифрлеу сөзі жапсырмасы
        ///     label10 - бүйір мәзір жабу символы (<)
        ///     label11 - бүйір мәзір ашу символы (>)
        ///     label14 - баптаулар сөзі жапсырмасы
        /// }
        /// panel2 - негізгі орта (panel3 және panel4 панельдерін қамтиді)
        /// {
        ///     panel3 - шифрлеу/дешифрлеу операциялары орындалатын панель
        ///     {
        ///         button1 - шифрлеу/дешифрлеу батырмасы
        ///         button2 - кездейсоқ кілт жасау батырмасы
        ///         button3 - латын әріптерді таңдау батырмасы
        ///         button4 - кириллица әріптерді таңдау батырмасы
        ///         button8 - файл таңдау батырмасы
        ///    
        ///         label4 - кілт сөзі жапсырмасы
        ///         label5 - ашық мәтін/шифр мәтін сөзі жапсырмасы
        ///         label6 - көмекші жапсырмасы (?)
        ///         label7 - көмекші жапсырмасы (?)
        ///         label8 - кілт сөзі жапсырмасы (көмекші)
        ///         label9 - ашық мәтін/шифр мәтін сөзі жапсырмасы (көмекші)
        ///         label12 - кездейсоқ кілт сөзі жапсырмасы
        ///         label13 - кілт ұзындығын көрсетуші сөзі жапсырмасы
        ///         label17 - файл таңдау сөзі жапсырмасы
        ///    
        ///         textBox1 - кілт енгізу өрісі
        ///         textBox2 - ашық мәтін/шифр мәтін енгізу өрісі
        ///         textBox3 - нәтиже өрісі
        ///         textBox4 - қосымша баптаулар нәтиже өрісі
        ///    
        ///         checkBox6 - txt жалаулары
        ///         checkBox7 - jpg жалаулары
        ///         checkBox8 - png жалаулары
        ///     }
        ///     panel4 - баптауларды қамтитін панель
        ///     {
        ///     
        ///         button5 - қазақ тілі таңдау батырмасы
        ///         button6 - орыс тілі таңдау батырмасы
        ///         button7 - ағылшын тілі таңдау батырмасы
        ///     
        ///         label15 - тіл сөзі жапсырмасы
        ///         label16 - қосымша баптаулар сөз жапсырмасы
        ///     
        ///         checkBox1 - кілт қосымша баптауы
        ///         checkBox2 - раундық кілт қосымша баптауы
        ///         checkBox3 - операцияның орындалу уақыты қосымша баптауы
        ///         checkBox4 - мәтін қосымша баптауы
        ///         checkBox5 - барлығы қосымша баптауы
        ///     }
        /// }
        /// </summary>


        private OpenFileDialog openFileDialog1;
        bool withFile = false;      // Егер шифрлеу кезінде файл таңдалатын болса
        string extension = "txt";   // Дешифрленген кезде әдеткі формат
        bool isTxt = true;          // 
        bool isJpg = false;         //  > Таңдалған файл форматы
        bool isPng = false;         // 
        byte[] fileName;            // Файл аты
        //  Бүйір мәзір
        bool isSidebarOpen = true;  // Бүйір мәзір ашылуын тексеруші
        bool isEncryption = true;   //
        bool isDecription = false;  //  > Қай операция таңдалғанын тексеруші
        bool isSettings = false;    //

        // Таңдалған тіл тексеруші
        bool KZ = false;
        bool RU = false;
        bool ENG = true;

        // Таңдалған тіл бойынша қажетті ресурсты таңдайтын аудармашы
        ResourceManager rm = new ResourceManager("Diploma_Project.en_local", Assembly.GetExecutingAssembly());

        //  Кілттің тілі
        bool isLatin = true;
        bool isCyrillic = false;

        //  Баптаулар жалаушалар (чекбокстар)
        bool settingsChanged = false;
        bool isAll = false;
        bool isRaundKeys = false;
        bool isKey = false;
        bool isExecutionTime = false;
        bool isPlainText = false;

        //  Толтырғыштар (placeholders)
        string enterKey = "Enter the key";
        string plainText = "Enter the text to encrypt/decrypt";
        string result = "The result is displayed here";

        // Баптаулар жалаушыларын қамтитын сөздік
        Dictionary<string, bool> checkboxes = new Dictionary<string, bool>()
        {
            ["settingsChanged"] = false,
            ["isPlainText"] = false,
            ["isKey"] = false,
            ["isRaundKeys"] = false,
            ["isExecutionTime"] = false,
            ["isAll"] = false
        };

        public MainForm()
        {
            InitializeComponent();
        }


        private void label6_MouseHover(object sender, EventArgs e)
        {
            this.label8.Visible = true;
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            this.label8.Visible = false;
        }

        private void label7_MouseHover(object sender, EventArgs e)
        {
            this.label9.Visible = true;
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            this.label9.Visible = false;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            this.isSidebarOpen = false;

            this.panel3.Size = new System.Drawing.Size(729, 375);
            this.panel3.Location = new System.Drawing.Point(37, 12);
            this.panel4.Size = new System.Drawing.Size(729, 375);
            this.panel4.Location = new System.Drawing.Point(37, 12);
            this.panel1.Size = new System.Drawing.Size(30, 329);
            this.label2.Size = new System.Drawing.Size(30, 30);
            this.label3.Size = new System.Drawing.Size(30, 30);
            this.label14.Size = new System.Drawing.Size(30, 30);
            if (this.settingsChanged)
            {
                this.textBox3.Size = new System.Drawing.Size(455, 84);
                this.textBox4.Size = new System.Drawing.Size(455, 188);
            }
            else
            {
                this.textBox3.Size = new System.Drawing.Size(455, 272);
            }
            

            this.button1.Location = new System.Drawing.Point(512, 317);
            this.label1.Text = this.rm.GetString("G");
            this.label2.Text = this.rm.GetString("E");
            this.label3.Text = this.rm.GetString("D");
            this.label14.Text = this.rm.GetString("S");
            this.label10.Visible = false;
            this.label11.Visible = true;
        }

        private void label11_Click(object sender, EventArgs e)
        {
            this.isSidebarOpen = true;

            this.panel3.Size = new System.Drawing.Size(615, 375);
            this.panel3.Location = new System.Drawing.Point(155, 12);
            this.panel4.Size = new System.Drawing.Size(615, 375);
            this.panel4.Location = new System.Drawing.Point(155, 12);
            this.panel1.Size = new System.Drawing.Size(150, 329);
            this.label2.Size = new System.Drawing.Size(150, 30);
            this.label3.Size = new System.Drawing.Size(150, 30);
            this.label14.Size = new System.Drawing.Size(150, 30);
            if (this.settingsChanged)
            {
                this.textBox3.Size = new System.Drawing.Size(345, 84);
                this.textBox4.Size = new System.Drawing.Size(345, 188);
            }
            else
            {
                this.textBox3.Size = new System.Drawing.Size(345, 272);
            }
            

            this.button1.Location = new System.Drawing.Point(403, 317);

            this.label1.Text = rm.GetString("GOST 34-12");
            this.label2.Text = rm.GetString("Encryption");
            this.label3.Text = rm.GetString("Decryption");
            this.label14.Text = rm.GetString("Settings");
            this.label10.Visible = true;
            this.label11.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            if (this.textBox1.ForeColor != System.Drawing.SystemColors.ScrollBar && this.textBox2.ForeColor != System.Drawing.SystemColors.ScrollBar)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                main Gost = new main();                            //Создание экземпляра класса Кузнечик 
                string textToEncrypt = this.textBox2.Text;
                string password = this.textBox1.Text;
                if(((textToEncrypt == "" && this.textBox2.Text == this.rm.GetString("Enter the text to encrypt/decrypt")) || (password == "" && this.textBox1.Text == this.rm.GetString("Enter the key"))) && !withFile)
                {
                    this.button1.Cursor = System.Windows.Forms.Cursors.No;
                    return;
                } else
                {
                    this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
                }

                
                string operation = "";
                if (isEncryption)
                {
                    if (withFile)
                    {
                        byte[] EncryptedFile = Gost.GostEncript(fileName, Encoding.Default.GetBytes(password)); //Получение массива байт зашифрованного файла
                        string nameOfFile = $"Encrfile{rand.Next(1, 1000)}.anet";
                        File.WriteAllBytes(nameOfFile, EncryptedFile);
                        this.textBox3.Text = $"{this.rm.GetString("The file is encrypted and saved here")}: {@"C:\Users\moaton\source\repos\Diploma Project\Diploma Project\bin\Debug"} - {nameOfFile}";
                    } else
                    {
                        byte[] EncryptedText = Gost.GostEncript(Encoding.Default.GetBytes(textToEncrypt), Encoding.Default.GetBytes(password)); //Получение массива байт зашифрованного файла
                        string EncrText = Encoding.Default.GetString(EncryptedText);
                        this.textBox3.Text = EncrText;
                        this.textBox4.Text = "";
                    }
                    
                    operation = this.rm.GetString("The time of the encryption operation");
                }
                else
                {
                    if (withFile)
                    {
                        byte[] DecryptedFile = Gost.GostDecript(fileName, Encoding.Default.GetBytes(password)); //Получение массива байт расшифрованного файла
                        string nameOfFile = $"Decrfile{rand.Next(1, 1000)}.{extension}";
                        File.WriteAllBytes(nameOfFile, DecryptedFile);
                        this.textBox3.Text = $"{this.rm.GetString("The file is decrypted and saved here")}: {@"C:\Users\moaton\source\repos\Diploma Project\Diploma Project\bin\Debug"} - {nameOfFile}";
                    } else
                    {
                        byte[] DecryptedText = Gost.GostDecript(Encoding.Default.GetBytes(textToEncrypt), Encoding.Default.GetBytes(password)); //Получение массива байт расшифрованного файла
                        string DecrText = Encoding.Default.GetString(DecryptedText);

                        this.textBox3.Text = DecrText;
                        this.textBox4.Text = "";
                    }
                   
                    operation = this.rm.GetString("The time of the decryption operation");
                }
                stopWatch.Stop();
                this.textBox1.Text = Gost.keyText;
                if (this.settingsChanged)
                {
                    foreach (var Checkbox in this.checkboxes)
                    {
                        switch (Checkbox.Key)
                        {
                            case "isKey":
                                if (Checkbox.Value)
                                {
                                    this.textBox4.Text += this.rm.GetString("Key") + ": " + Environment.NewLine + Gost.keyByte + Environment.NewLine  + Environment.NewLine;
                                }
                                break;
                            case "isRaundKeys":
                                if (Checkbox.Value)
                                {
                                    this.textBox4.Text += this.rm.GetString("Array of iterative keys") + Environment.NewLine + Gost.getRoundKeys() + Environment.NewLine  + Environment.NewLine;
                                }
                                break;
                            case "isExecutionTime":
                                if (Checkbox.Value)
                                {
                                    this.textBox4.Text += operation + ": " + stopWatch.ElapsedTicks + " " + this.rm.GetString("Tact") + Environment.NewLine  + Environment.NewLine;
                                }
                                break;
                            case "isPlainText":
                                if (Checkbox.Value)
                                {
                                    this.textBox4.Text +=  this.rm.GetString("Plain Text (HEX)") + ": " + BitConverter.ToString(Encoding.Default.GetBytes(textToEncrypt)) + Environment.NewLine + this.rm.GetString("Byte sequence") + ": " + string.Join(" ", Encoding.Default.GetBytes(textToEncrypt)) + Environment.NewLine + this.rm.GetString("Numbers of blocks") + ": " + Gost.numbersOfBlock + Environment.NewLine + this.rm.GetString("Plain Text (HEX)") + ": " + Gost.plainTextLengthened + Environment.NewLine + Environment.NewLine;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            string keyGen = "";
            string alphabet = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_-()&@";
            if (isCyrillic)
            {
                alphabet = "1234567890АВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя_-()&@";
            }
            for(int i = 0; i < 32; i++)
            {                
                keyGen += alphabet[rand.Next(alphabet.Length)];
            }
            this.label13.Text = (keyGen.Length * 8) + " " + this.rm.GetString("Bit");
            this.textBox1.Text = keyGen;
            this.textBox1.ForeColor = System.Drawing.SystemColors.WindowText;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (this.textBox1.Text != "" && this.textBox1.Text != this.rm.GetString("Enter the key") && this.textBox1.ForeColor == System.Drawing.SystemColors.ScrollBar)
            {
                this.textBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            }
            if (tb.Text.Length > 32)
            {
                tb.Text = tb.Text.Substring(0, 32);
            }
            if (((this.textBox1.Text == "" && this.textBox1.Text == this.rm.GetString("Enter the text to encrypt/decrypt")) || (tb.Text == "" && this.textBox1.Text == this.rm.GetString("Enter the key"))) && !withFile)
            {
                this.button1.Cursor = System.Windows.Forms.Cursors.No;
            }
            else
            {
                this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            }
            this.label13.Text = tb.Text.Length * 8 + " " + this.rm.GetString("Bit");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            fileName = null;
            this.button8.Text = this.rm.GetString("Choose");
            this.textBox2.Enabled = true;
            this.withFile = false;
            this.checkBox1.Enabled = true;
            this.checkBox2.Enabled = true;
            this.checkBox3.Enabled = true;
            this.checkBox4.Enabled = true;
            this.checkBox5.Enabled = true;
            isEncryption = true;
            isDecription = false;
            isSettings = false;
            this.panel3.Visible = true;
            this.panel4.Visible = false;
            this.label5.Text = this.rm.GetString("Message");
            this.label9.Text = this.rm.GetString("The message that needs to be encrypted");
            this.button3.Visible = true;
            this.button4.Visible = true;
            this.checkBox6.Visible = false;
            this.checkBox7.Visible = false;
            this.checkBox8.Visible = false;
            if (this.RU)
            {
                this.label6.Location = new Point(75, 16);
                this.label8.Location = new Point(90, 10);
                
                this.label7.Location = new Point(80, 93);
                this.label9.Location = new Point(100, 90);

            } else if (this.KZ)
            {
                this.label6.Location = new Point(55, 16);
                this.label8.Location = new Point(70, 10);

                this.label7.Location = new Point(75, 93);
                this.label9.Location = new Point(90, 90);
            }
            else
            {
                this.label7.Location = new Point(106, 93);
                this.label9.Location = new Point(122, 90);
            }
            this.textBox2.Text = this.rm.GetString("Enter the text to encrypt/decrypt");
            this.textBox3.Text = this.rm.GetString("The result is displayed here");
            this.textBox2.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.textBox3.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.button1.Text = this.rm.GetString("Encrypt");
            this.button2.Visible = true;
            this.label12.Visible = true;
            this.label3.BackColor = Color.FromArgb(75, 75, 75);
            this.label2.BackColor = Color.FromArgb(106, 106, 106);
            this.label14.BackColor = Color.FromArgb(75, 75, 75);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            fileName = null;
            this.button8.Text = this.rm.GetString("Choose");
            this.textBox2.Enabled = true;
            this.withFile = false;
            this.checkBox1.Enabled = true;
            this.checkBox2.Enabled = true;
            this.checkBox3.Enabled = true;
            this.checkBox4.Enabled = true;
            this.checkBox5.Enabled = true;
            isDecription = true;
            isEncryption = false;
            isSettings = false;
            this.panel3.Visible = true;
            this.panel4.Visible = false;
            this.label5.Text = this.rm.GetString("Encrypted Text");
            this.label9.Text = this.rm.GetString("The message that needs to be decrypted");
            this.button3.Visible = false;
            this.button4.Visible = false;
            this.checkBox6.Visible = true;
            this.checkBox7.Visible = true;
            this.checkBox8.Visible = true;

            isTxt = true;
            this.extension = "txt";
            this.checkBox6.Checked = true;


            if (this.RU)
            {
                this.label6.Location = new Point(75, 16);
                this.label8.Location = new Point(90, 10);

                this.label7.Location = new Point(230, 93);
                this.label9.Location = new Point(245, 90);
            } else if (this.KZ)
            {
                this.label7.Location = new Point(160, 93);
                this.label9.Location = new Point(175, 90);
            }
            else
            {
                this.label7.Location = new Point(155, 93);
                this.label9.Location = new Point(170, 90);
            }
            this.textBox2.Text = this.rm.GetString("Enter the text to encrypt/decrypt");
            this.textBox3.Text = this.rm.GetString("The result is displayed here");
            this.textBox2.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.textBox3.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.button1.Text = this.rm.GetString("Decrypt");
            this.button2.Visible = false;
            this.label12.Visible = false;
            this.label2.BackColor = Color.FromArgb(75, 75, 75);
            this.label3.BackColor = Color.FromArgb(106, 106, 106);
            this.label14.BackColor = Color.FromArgb(75, 75, 75);
        }

        private void label14_Click(object sender, EventArgs e)
        {
            isDecription = false;
            isEncryption = false;
            isSettings = true;
            this.panel3.Visible = false;
            this.panel4.Visible = true;
            this.label2.BackColor = Color.FromArgb(75, 75, 75);
            this.label3.BackColor = Color.FromArgb(75, 75, 75);
            this.label14.BackColor = Color.FromArgb(106, 106, 106);
            this.button3.Visible = false;
            this.button4.Visible = false;
            this.checkBox6.Visible = false;
            this.checkBox7.Visible = false;
            this.checkBox8.Visible = false;
        }

        private void label3_MouseHover(object sender, EventArgs e)
        {
            if (!isDecription)
            {
                this.label3.BackColor = Color.FromArgb(45, 45, 45);                
            }
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            if (!isDecription)
            {
                this.label3.BackColor = Color.FromArgb(75, 75, 75);
            }
            else
            {
                this.label3.BackColor = Color.FromArgb(106, 106, 106);
            }
            
        }

        private void label2_MouseHover(object sender, EventArgs e)
        {
            if (!isEncryption)
            {
                this.label2.BackColor = Color.FromArgb(45, 45, 45);
            }
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            if (!isEncryption)
            {
                this.label2.BackColor = Color.FromArgb(75, 75, 75);
            }
            else
            {
                this.label2.BackColor = Color.FromArgb(106, 106, 106);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            isLatin = true;
            isCyrillic = false;

            this.button3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.button4.BackColor = System.Drawing.SystemColors.ControlLight;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            isLatin = false;
            isCyrillic = true;

            this.button3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button4.BackColor = System.Drawing.SystemColors.ControlDark;
        }

        public void checkAllCheckBoxes()
        {
            int count = 0;
            foreach (var Checkbox in this.checkboxes.ToArray())
            {
                switch (Checkbox.Key)
                {
                    case "isPlainText":
                        if (this.checkboxes[$"{Checkbox.Key}"])
                        {
                            count++;
                        }
                        break;
                    case "isKey":
                        if (this.checkboxes[$"{Checkbox.Key}"])
                        {
                            count++;
                        }
                        break;
                    case "isRaundKeys":
                        if (this.checkboxes[$"{Checkbox.Key}"])
                        {
                            count++;
                        }
                        break;
                    case "isExecutionTime":
                        if (this.checkboxes[$"{Checkbox.Key}"])
                        {
                            count++;
                        }
                        break;
                }
                if(count != 4)
                {
                    this.checkboxes[$"isAll"] = false;
                    this.checkBox5.Checked = false;
                }
                    
            }
        }
        //All checkbox
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if(this.isAll)
            {
                this.settingsChanged = false;
                this.isAll = false;
                this.isKey = false;
                this.isRaundKeys = false;
                this.isExecutionTime = false;
                this.isPlainText = false;

                foreach (var Checkbox in this.checkboxes.ToArray())
                {
                    this.checkboxes[Checkbox.Key] = false;
                }

                this.checkBox1.Checked = false;
                this.checkBox2.Checked = false;
                this.checkBox3.Checked = false;
                this.checkBox4.Checked = false;
                this.checkBox5.Checked = false;

                this.textBox4.Visible = false;
                if (this.isSidebarOpen)
                {
                    this.textBox3.Size = new System.Drawing.Size(345, 272);
                }
                else
                {
                    this.textBox3.Size = new System.Drawing.Size(455, 272);
                }
            }
            else
            {
                this.settingsChanged = true;
                this.isAll = true;
                this.isKey = true;
                this.isRaundKeys = true;
                this.isExecutionTime = true;
                this.isPlainText = true;

                foreach (var Checkbox in this.checkboxes.ToArray())
                {
                    this.checkboxes[$"{Checkbox.Key}"] = true;
                }

                this.checkBox1.Checked = true;
                this.checkBox2.Checked = true;
                this.checkBox3.Checked = true;
                this.checkBox4.Checked = true;
                this.checkBox5.Checked = true;

                this.textBox4.Visible = true;
                if (this.isSidebarOpen)
                {
                    this.textBox3.Size = new System.Drawing.Size(345, 84);
                    this.textBox4.Size = new System.Drawing.Size(345, 188);
                }
                else
                {
                    this.textBox3.Size = new System.Drawing.Size(455, 84);
                    this.textBox4.Size = new System.Drawing.Size(455, 188);
                }
            }
        }



        //Key checkbox
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked)
            {
                this.checkboxes["isKey"] = true;
                this.isKey = true;
                this.settingsChanged = true;

                this.textBox4.Visible = true;
                if (this.isSidebarOpen)
                {
                    this.textBox3.Size = new System.Drawing.Size(345, 84);
                    this.textBox4.Size = new System.Drawing.Size(345, 188);
                }
                else
                {
                    this.textBox3.Size = new System.Drawing.Size(455, 84);
                    this.textBox4.Size = new System.Drawing.Size(455, 188);
                }
            } else
            {
                this.checkboxes["isKey"] = false;
                this.isKey = false;
                int count = 0;
                foreach (var Checkbox in this.checkboxes)
                {
                    if (Checkbox.Value)
                    {
                        count++;
                    }
                }
                if(count == 0)
                {
                    this.settingsChanged = false;
                    this.textBox4.Visible = false;
                    if (this.isSidebarOpen)
                    {
                        this.textBox3.Size = new System.Drawing.Size(345, 272);
                    }
                    else
                    {
                        this.textBox3.Size = new System.Drawing.Size(455, 272);
                    }
                }
            }
            //checkAllCheckBoxes();
        }
        //Round Keys checkbox
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked)
            {
                this.checkboxes["isRaundKeys"] = true;
                this.isRaundKeys = true;
                this.settingsChanged = true;

                this.textBox4.Visible = true;
                if (this.isSidebarOpen)
                {
                    this.textBox3.Size = new System.Drawing.Size(345, 84);
                    this.textBox4.Size = new System.Drawing.Size(345, 188);
                }
                else
                {
                    this.textBox3.Size = new System.Drawing.Size(455, 84);
                    this.textBox4.Size = new System.Drawing.Size(455, 188);
                }
            }
            else
            {
                this.checkboxes["isRaundKeys"] = false;
                this.isRaundKeys = false;
                int count = 0;
                foreach (var Checkbox in this.checkboxes)
                {
                    if (Checkbox.Value)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    this.settingsChanged = false;
                    this.textBox4.Visible = false;
                    if (this.isSidebarOpen)
                    {
                        this.textBox3.Size = new System.Drawing.Size(345, 272);
                    }
                    else
                    {
                        this.textBox3.Size = new System.Drawing.Size(455, 272);
                    }
                }
            }
        }
        //Execution time checkbox
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked)
            {
                this.checkboxes["isExecutionTime"] = true;
                this.isExecutionTime = true;
                this.settingsChanged = true;

                this.textBox4.Visible = true;
                if (this.isSidebarOpen)
                {
                    this.textBox3.Size = new System.Drawing.Size(345, 84);
                    this.textBox4.Size = new System.Drawing.Size(345, 188);
                }
                else
                {
                    this.textBox3.Size = new System.Drawing.Size(455, 84);
                    this.textBox4.Size = new System.Drawing.Size(455, 188);
                }
            }
            else
            {
                this.checkboxes["isExecutionTime"] = false;
                this.isExecutionTime = false;
                int count = 0;
                foreach (var Checkbox in this.checkboxes)
                {
                    if (Checkbox.Value)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    this.settingsChanged = false;
                    this.textBox4.Visible = false;
                    if (this.isSidebarOpen)
                    {
                        this.textBox3.Size = new System.Drawing.Size(345, 272);
                    }
                    else
                    {
                        this.textBox3.Size = new System.Drawing.Size(455, 272);
                    }
                }
            }
        }
        //Plain text checkbox
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked)
            {
                this.checkboxes["isPlainText"] = true;
                this.isPlainText = true;
                this.settingsChanged = true;

                this.textBox4.Visible = true;
                if (this.isSidebarOpen)
                {
                    this.textBox3.Size = new System.Drawing.Size(345, 84);
                    this.textBox4.Size = new System.Drawing.Size(345, 188);
                }
                else
                {
                    this.textBox3.Size = new System.Drawing.Size(455, 84);
                    this.textBox4.Size = new System.Drawing.Size(455, 188);
                }
            }
            else
            {
                this.checkboxes["isPlainText"] = false;
                this.isPlainText = false;
                int count = 0;
                foreach (var Checkbox in this.checkboxes)
                {
                    if (Checkbox.Value)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    this.settingsChanged = false;
                    this.textBox4.Visible = false;
                    if (this.isSidebarOpen)
                    {
                        this.textBox3.Size = new System.Drawing.Size(345, 272);
                    }
                    else
                    {
                        this.textBox3.Size = new System.Drawing.Size(455, 272);
                    }
                }
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(this.textBox1.Text == this.rm.GetString("Enter the key"))
            {
                this.textBox1.ForeColor = System.Drawing.SystemColors.WindowText;
                this.textBox1.Text = "";
            }
            
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if(this.textBox1.Text == "" || this.textBox1.TextLength == 0)
            {
                this.textBox1.ForeColor = System.Drawing.SystemColors.ScrollBar;
                this.textBox1.Text = this.rm.GetString("Enter the key");
            }
            
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.textBox2.Text == this.rm.GetString("Enter the text to encrypt/decrypt"))
            {
                this.textBox2.ForeColor = System.Drawing.SystemColors.WindowText;
                this.textBox2.Text = "";
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (this.textBox2.Text == "" || this.textBox2.TextLength == 0)
            {
                this.textBox2.ForeColor = System.Drawing.SystemColors.ScrollBar;
                this.textBox2.Text = rm.GetString("Enter the text to encrypt/decrypt");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (this.textBox2.ForeColor != System.Drawing.SystemColors.ScrollBar)
            {
                this.textBox2.ForeColor = System.Drawing.SystemColors.WindowText;
            }
            if (((tb.Text == "" && this.textBox2.Text == this.rm.GetString("Enter the text to encrypt/decrypt")) || (this.textBox1.Text == "" && this.textBox1.Text == this.rm.GetString("Enter the key"))) && !withFile)
            {
                this.button1.Cursor = System.Windows.Forms.Cursors.No;
            }
            else
            {
                this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            }
        }
        public void UpdateLang(string name)
        {
            this.rm = new ResourceManager($"Diploma_Project.{name}", Assembly.GetExecutingAssembly());

            this.textBox1.Text = this.rm.GetString("Enter the key");
            this.textBox1.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.textBox2.Text = this.rm.GetString("Enter the text to encrypt/decrypt");
            this.textBox1.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.textBox3.Text = this.rm.GetString("The result is displayed here");
            this.textBox3.ForeColor = System.Drawing.SystemColors.ScrollBar;

            this.label1.Text = this.rm.GetString("GOST 34-12");
            this.label2.Text = this.rm.GetString("Encryption");
            this.label3.Text = this.rm.GetString("Decryption");
            this.label14.Text = this.rm.GetString("Settings");
            this.label15.Text = this.rm.GetString("Language");
            this.label16.Text = this.rm.GetString("Additionally");


            this.checkBox1.Text = this.rm.GetString("Key");
            this.checkBox2.Text = this.rm.GetString("Round Keys");
            this.checkBox3.Text = this.rm.GetString("Operation execution time");
            this.checkBox4.Text = this.rm.GetString("Plain Text (HEX)");
            this.checkBox5.Text = this.rm.GetString("All");

            if (!this.isSidebarOpen)
            {
                this.label1.Text = this.rm.GetString("G");
                this.label2.Text = this.rm.GetString("E");
                this.label3.Text = this.rm.GetString("D");
                this.label14.Text = this.rm.GetString("S");
            }

            this.label4.Text = this.rm.GetString("Key");
            this.label5.Text = this.rm.GetString("Message");
            this.label8.Text = this.rm.GetString("The key must be 256 bits");
            this.label9.Text = this.rm.GetString("The message that needs to be encrypted");
            this.label12.Text = this.rm.GetString("Random Key");
            this.label13.Text = 0 + " " + this.rm.GetString("Bit");

            this.label17.Text = this.rm.GetString("Select a file");
            this.button8.Text = this.rm.GetString("Choose");



            this.button2.Text = this.rm.GetString("Generate");
            this.button3.Text = this.rm.GetString("Latin");
            this.button4.Text = this.rm.GetString("Cyrillic");

        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.KZ = true;
            this.RU = false;
            this.ENG = false;

            this.UpdateLang("kz_local");
            this.button5.BackColor = System.Drawing.SystemColors.ControlDark;
            this.button6.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button7.BackColor = System.Drawing.SystemColors.ControlLight;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.KZ = false;
            this.RU = true;
            this.ENG = false;

            this.UpdateLang("ru_local");


            this.button5.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button6.BackColor = System.Drawing.SystemColors.ControlDark;
            this.button7.BackColor = System.Drawing.SystemColors.ControlLight;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.KZ = false;
            this.RU = false;
            this.ENG = true;

            this.UpdateLang("en_local");

            this.button5.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button6.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button7.BackColor = System.Drawing.SystemColors.ControlDark;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            //// получаем выбранный файл
            string filename = openFileDialog1.FileName;
            Console.WriteLine(filename.Split(Convert.ToChar(92))[filename.Split(Convert.ToChar(92)).Length-1]);
            this.textBox3.Text = $"{this.rm.GetString("The file is selected")}: {filename}{Environment.NewLine}{this.rm.GetString("Name of File")}: {filename.Split(Convert.ToChar(92))[filename.Split(Convert.ToChar(92)).Length - 1].Split(Convert.ToChar(46))[0]}{Environment.NewLine}{this.rm.GetString("Format")}: {filename.Split(Convert.ToChar(92))[filename.Split(Convert.ToChar(92)).Length - 1].Split(Convert.ToChar(46))[1]}";
            Console.WriteLine(filename);
            //// читаем файл в строку
            //string fileText = System.IO.File.ReadAllText(filename);
            //this.textBox2.Text = fileText;

            fileName = File.ReadAllBytes(filename);
           

            this.textBox2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox2.Enabled = false;

            this.button8.Text = this.rm.GetString("Choosed");

            this.withFile = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Checked = false;
            this.checkBox2.Enabled = false;
            this.checkBox2.Checked = false;
            this.checkBox3.Enabled = false;
            this.checkBox3.Checked = false;
            this.checkBox4.Enabled = false;
            this.checkBox4.Checked = false;
            this.checkBox5.Enabled = false;
            this.checkBox5.Checked = false;
            if (this.textBox1.Text != "" && this.textBox1.Text != this.rm.GetString("Enter the key"))
            {
                this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            } else
            {
                this.button1.Cursor = System.Windows.Forms.Cursors.No;
            }
            //MessageBox.Show("Файл открыт");
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked)
            {
                this.extension = "txt";
                isTxt = true;
                isJpg = false;
                isPng = false;
                this.checkBox7.Checked = false;
                this.checkBox8.Checked = false;
            }
            else
            {
                isTxt = false;
                checkFormat();
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked)
            {
                this.extension = "jpg";
                isTxt = false;
                isJpg = true;
                isPng = false;
                this.checkBox6.Checked = false;
                this.checkBox8.Checked = false;
            } else
            {
                isJpg = false;
                checkFormat();
            }
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked)
            {
                this.extension = "png";
                isTxt = false;
                isJpg = false;
                isPng = true;
                this.checkBox6.Checked = false;
                this.checkBox7.Checked = false;
            }
            else
            {
                isPng = false;
                checkFormat();
            }
        }
        public void checkFormat()
        {
            if(!isPng && !isJpg && !isTxt)
            {
                isTxt = true;
                this.extension = "txt";
                this.checkBox6.Checked = true;
            }
        }
    }
}
