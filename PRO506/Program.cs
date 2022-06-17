﻿using PRO506;
using System;
using System.Collections.Generic;
using System.IO;

namespace Task
{
    /*
     *  Main struct that holds all the functionality
     *  of the code i.e. 'Program'
     */
    struct Program
    {
        //all taxation values for each threashold
        private readonly TaxRates _taxRates = new TaxRates
        {
            UpTo14000 = 0.105,
            Over14000UpTo48000 = 0.175,
            Over48000UpTo70000 = 0.3,
            Over70000UpTo180000 = 0.33,
            Over180000 = 0.39
        };
        private readonly TaxCalculator _taxCalculator;
        //List for employees and their payrolls (or dynamic arrays)
        private List<Employee> _employees;

        /*
         * Constructor to initialize the lists
         * and read employees from the file.
         * Then calculate fortnight payroll 
         * for each employee
         * After than runs the program.
         */
        public Program()
        {
            _employees = new List<Employee>();//Initializes employee list
            _taxCalculator = new TaxCalculator(_taxRates);// init tax calculator
            readEmployees();//Read employees from the file
            CalculateFortnight();//Calculate fortnite of all employees
            runner();//Runs the main loop of the program
        }

        /*
         * Basically the runner code it won't 
         * exit till user wants to exit. 
         * exit is the variable that holds the
         * information if user wants to exit or 
         * not. So do, while not exit ( user dont want to exit)
         * show the menu and read user input. As exit is true
         * !exit becomes false and false breaks the loop.
         */
        public void runner()
        {
            bool exit = false;//Exit variable to exit the loop
            do//Do run the program 1st time and if user wants to exit then exit the program 
            {
                displayMenu();//Display menu
                processUserRequest(userInput(), ref exit);//Process user request upon input
            } while (!exit);//Exit when user wants to exit
        }

        /*
         * Displays the menu to the user
         */
        public void displayMenu()
        {
            Console.WriteLine(PrintTitle("WELCOME TO NEW KIWI GARAGE PAYROLL SYSTEM"));//Write main title
            Console.WriteLine("\n\n");//Next line and next line
            Console.WriteLine("1. Fortnight payroll calculation");
            Console.WriteLine("2. Sort and display the employee records");
            Console.WriteLine("3. Search for an employee");
            Console.WriteLine("4. Save into text file");
            Console.WriteLine("0. Exit");
            Console.WriteLine("\n\n");
            Console.WriteLine(PrintTitle("Please select an option"));//Print title to make user enter the input
        }

        /*
         * Handles user input.
         * Reads user key and converts to
         * an int and returns the value.
         */
        public ConsoleKey userInput() => Console.ReadKey().Key;//returns expresseion after reading the user key

        /*
         * Reads employees from a file naming employees.txt.
         * Creates a fileStream the file modes are open and read.
         * After that returns the streamreader carret to the start i.e. 
         * SeekOrigin.Begin. Reads a line from the file till null.
         * As the format of file is 'id first last salary kivi'
         * which is a one line as a string. That string is then split
         * by spaces between the value that gives us an array of 5 values
         * strored in string[] employee. index 0 contains id, 1 contains 
         * first name and so on...
         * These values are stored in employees list as a new Employee().
         * After reading file contents, file is closed properly.
         */
        public void readEmployees()
        {
            FileStream fileStream = new FileStream("employees.txt", FileMode.Open, FileAccess.Read);//Create file stream object with open and read modes
            StreamReader streamReader = new StreamReader(fileStream);//Create file reader object
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);//Return carret to the start of the file
            for (string line = streamReader.ReadLine(); line != null; line = streamReader.ReadLine())//Read line and store in the string. Read till the read line is not empty.
            {
                string[] employee = line.Split(' ');//Split the read values by a space and returns each value to the array
                _employees.Add(new Employee(int.Parse(employee[0]), employee[1], employee[2], double.Parse(employee[3]), float.Parse(employee[4]), new Payroll()));//Store the values in the employee
            }
            streamReader.Close();//Close the reader 
            fileStream.Close();//Close the file
        }

        /*
         * Process user request. Parameters are an option
         * selected by user and refrence of exit variable.
         * case 1 :
         *      calculate the payroll
         *      and print it
         * case 2:
         *      sort and print employee
         * case 3:
         *      search for an employee
         * case 4: 
         *      save data
         * default:
         *      exit
         */
        public void processUserRequest(ConsoleKey key, ref bool exit)
        {
            Console.Clear();
            switch (key)
            {
                case ConsoleKey.D1://Key is 1
                case ConsoleKey.NumPad1://Key is num1
                    PrintPayroll();//Calculate payroll
                    break;
                case ConsoleKey.D2://Key is 2
                case ConsoleKey.NumPad2://Key is num2
                    OrderEmployeesById();//Sort the emplyees
                    PrintEmployee();//Print the employees
                    break;
                case ConsoleKey.D3://Key is 3
                case ConsoleKey.NumPad3://Key is num3
                    SearchEmployee();//Search the employee
                    break;
                case ConsoleKey.D4://Key is 4
                case ConsoleKey.NumPad4://Key is num4
                    SaveData();//Save data to file
                    break;
                default://Any other key pressed
                    exit = true;//Exit the loop or program
                    return;
            }
            Console.WriteLine("Press any key to continue....");//Write instruction line
            Console.ReadKey();//Wait for the key to be pressed
            Console.Clear();//Clear the console
        }
        //Calculate each employee's fortnight
        public void CalculateFortnight()
        {
            SetPayroll(_employees);
        }

        //Print payroll of searched employee
        public void PrintPayroll()
        {
            //Print Employee
            PrintEmployee();
            //Print Title
            Console.WriteLine(PrintTitle("ENTER ID"));
            //Search for the employee against his ID
            string idInput = Console.ReadLine();

            bool res;
            int userIdConverted;
            if(! int.TryParse(idInput, out userIdConverted))
            {
                Console.WriteLine($"{idInput} is not a valid id");
                return;
            }

            Employee? employee = search(userIdConverted);
            //Clear the console
            Console.Clear();
            //If employee was found i.e. not null
            //Then print employee and his payroll
            if (employee != null)
            {
                Console.WriteLine(((Employee)employee).Print());//Print employees
                Console.WriteLine(((Employee)employee).Payroll.Print());//Print their payroll
            }
            else
                //Otherwise print invalid id
                Console.WriteLine("INVALID ID!");
            //If user pressed s then saved other wise returns to menu
            Console.WriteLine("\nPRESS ANY KEY TO CONTINUE OR S TO SAVE!");
            if (Console.ReadKey().Key == ConsoleKey.S)//If key pressed is S
                SaveData();//Save the data
        }

        //Print all employees in form of a table
        public void PrintEmployee()
        {
            Console.WriteLine(PrintTitle("EMPLOYEES"));//Print employees title
            Console.WriteLine("\n");//Print next line
            Console.WriteLine(new String('-', 97));//Print repeated string'-' 97 times
            Console.WriteLine("|Employee ID\t|Employee First Name\t|Employee Last Name\t|Annual Income\t|KiwiSaver\t|");//Print table title
            Console.WriteLine(new String('-', 97));//Print repeated string'-' 97 times
            foreach (Employee employee in _employees)//For each employee in the employees
            {
                Console.WriteLine(employee.ToString());//Write each employee
                Console.WriteLine(new String('-', 97));//Print repeated string'-' 97 times
            }
            Console.WriteLine("\n");//print next line
        }

        //Sort the employees
        public void OrderEmployeesById()
        {
            _employees = _employees.OrderBy(x => x.Id).ToList();
        }

        //Seach for employee and print it
        public void SearchEmployee()
        {
            Console.WriteLine("Please enter the id of employee : ");//print instruction
            Employee? employee = search(int.Parse(Console.ReadLine()));//Search for the employee at read id
            Console.Clear();//Clear the console
            if (employee != null)//If employee was null
            {
                Console.WriteLine(new string('-', 97));//Print repeated string'-' 97 times
                Console.WriteLine("|Employee ID\t|Employee First Name\t|Employee Last Name\t|Annual Income\t|KiwiSaver\t|");//Print table title
                Console.WriteLine(new string('-', 97));//Print repeated string'-' 97 times
                Console.WriteLine(((Employee)employee).ToString());//Print employee
                Console.WriteLine(new string('-', 97));//Print repeated string'-' 97 times
            }
            else
                Console.WriteLine("Invalid Id!");//Else the id was invalid
        }

        //Search for an employee against id and return it
        public Employee? search(int id)
        {
            foreach (Employee e in _employees)//For each employee in the employees
                if (e.Id == id)//If id matches
                    return e;//Return the employee
            return null;//Else return null
        }

        /*
         * Save the employee in weekly_payroll.txt and file
         * modes are OpenOrCreate and write.
         * After writing close the file
         */
        public void SaveData()
        {
            FileStream fileStream = new FileStream("weekly_payroll.txt", FileMode.OpenOrCreate, FileAccess.Write);//Create file reader with open, create and write
            StreamWriter streamWriter = new StreamWriter(fileStream);//Create file writer
            foreach (Employee employee in _employees)//For employee in employees
            {
                streamWriter.Write(employee.Print() + "\n");//Write employee in the file
                streamWriter.Write(employee.Payroll.Print() + "\n");//Write payroll in the file
            }
            streamWriter.Close();//Close the writer
            fileStream.Close();//Close the file
            Console.Clear();//Clear the console
            Console.WriteLine("Saved Successfully!");//Print message
        }

        //Print the title
        public static string PrintTitle(string title)
        {
            //Calculate padding on each side.
            //To write the title in the center, the 
            //padding should be equal on both sides.
            //padding means spaces
            int padding = 50 - title.Length / 2;//Put the title in the center so calculate spaces on each side
            return
                new string('-', 100) + "\n" +//Top border repeat string '-' 100 times
                String.Format("{0} {1} {2}", "".PadRight(padding), title, "".PadRight(padding)) + "\n" +//Print title with padding left and right
                new string('-', 100) + "\n";//Top border repeat string '-' 100 times

        }

        //Calculate payroll against an employee id and set its value
        public void SetPayroll(List<Employee> employees)
        {
            for (int i = 0; i < employees.Count(); i++)
            {
                double hourlyRate = _taxCalculator.CalculateHourlyRate(employees[i].Income);//Hourly rate of employee

                double annualKiwiSaver = _taxCalculator.CalculateKiwiSaver(employees[i].Income, employees[i].KiwiSaver);//Calcualte kivi saver value
                double annualTax = _taxCalculator.CalculateTax(employees[i].Income);//Tax variable
                                                                                    // int hoursWorked1 = 80;//Hours worked
                double netSalaryAnnualy = _taxCalculator.CalculateNetAnnualSalary(employees[i].Income, employees[i].KiwiSaver);

                double fortnlightlyNetSalary = _taxCalculator.CalculateFortnightlyPay(netSalaryAnnualy);
                double fortnlightlyKiwiSaver = _taxCalculator.CalculateFortnightlyPay(annualKiwiSaver);
                double fortnlightlyTax = _taxCalculator.CalculateFortnightlyPay(annualTax);
                double grossFortnlightlySalary = _taxCalculator.CalculateFortnightlyPay(employees[i].Income);

                employees[i].SetPayroll(new Payroll(hourlyRate: (int)Math.Round(hourlyRate),
                                           hoursWorked: TaxCalculator.HumberOfHoursForghtnigtlyPay,//use here constant because 80 hours is always 80 hours Forghtnigtly
                                           grossPay: (int)Math.Round(grossFortnlightlySalary),
                                           fortnightlyNetPayroll: (int)Math.Round(fortnlightlyNetSalary),
                                           tax: (int)Math.Round(fortnlightlyTax),
                                           kiwiSaver: (int)Math.Round(fortnlightlyKiwiSaver)));//Set employee payroll
            }


        }
    }

    //Structure Employee
    public struct Employee
    {
        //Properties
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Income { get; set; }
        public float KiwiSaver { get; set; }

        public Payroll Payroll { get; private set; }
        //Constructor
        public Employee(int id, string firstName, string lastName, double income, float kiwiSaver, Payroll payroll)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Income = income;
            this.KiwiSaver = kiwiSaver;
            this.Payroll = payroll;
        }
        //To String
        public string ToString()
        {
            return String.Format("|{0,-11}\t|{1,-19}\t|{2,-18}\t|{3,-13}\t|{4,-15}|", Id, FirstName, LastName, Income, KiwiSaver + "%");//String in the format 
        }

        public void SetPayroll(Payroll payroll)
        {
            this.Payroll = payroll;
        }
        //Print values
        public string Print()
        {
            return
                Program.PrintTitle("EMPLOYEE DETAILS") + "\n" +
                "Employee ID : " + this.Id + "\n" +
                "First Name : " + this.FirstName + "\n" +
                "Last Name : " + this.LastName + "\n" +
                "Yearly Income : " + this.Income + "\n" +
                "Kiwi Saver : " + this.KiwiSaver + "%" + "\n";
        }
    }

    //Payroll struct
    public struct Payroll
    {
        //Properties
        public int HourlyRate { get; set; }
        public int HoursWorked { get; set; }
        public float GrossPay { get; set; }
        public float FortnightlyNetPayroll { get; set; }

        public float Tax { get; set; }
        public float KiwiSaver { get; set; }

        //Constructors
        public Payroll(int hourlyRate, int hoursWorked, float grossPay, float fortnightlyNetPayroll, float tax, float kiwiSaver)
        {
            this.HourlyRate = hourlyRate;
            this.HoursWorked = hoursWorked;
            this.GrossPay = grossPay;
            this.FortnightlyNetPayroll = fortnightlyNetPayroll;
            this.Tax = tax;
            this.KiwiSaver = kiwiSaver;
        }

        //print payroll
        public string Print()
        {
            return
                Program.PrintTitle("PAYROLL") + "\n" +
                "Hourly Rate : " + this.HourlyRate + "$" + "\n" +
                "Hours worked : " + this.HoursWorked + "hr" + "\n" +
                "Gross Pay : " + this.GrossPay + "$" + "\n" +
                "DEDUCTIONS" + "\n" +
                "Tax : " + this.Tax + "$" + "\n" +
                "Kiwi Saver : " + this.KiwiSaver + "$" + "\n" +
                Program.PrintTitle("PAY") + "\n" +
                "Fortnightly Net Payroll : " + this.FortnightlyNetPayroll + "$" + "\n";
        }
    }

    class main
    {
        static void Main(string[] args)
        {
            new Program();
        }
    }
}
