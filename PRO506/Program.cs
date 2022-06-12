using System;
using System.Collections.Generic;
using System.IO;

namespace Task
{
    /*
     *  Main class that holds all the functionality
     *  of the code i.e. 'Program'
     */
    class Program
    {
        //List for employees and their payrolls (or dynamic arrays)
        private List<Employee> employees;

        /*
         * Constructor to initialize the lists
         * and read employees from the file.
         * Then calculate fortnight payroll 
         * for each employee
         * After than runs the program.
         */
        public Program()
        {
            employees = new List<Employee>();//Initializes employee list
            readEmployees();//Read employees from the file
            calculateFortnight();//Calculate fortnite of all employees
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
            Console.WriteLine(printTitle("WELCOME TO NEW KIWI GARAGE PAYROLL SYSTEM"));//Write main title
            Console.WriteLine("\n\n");//Next line and next line
            Console.WriteLine("1. Fortnight payroll calculation");
            Console.WriteLine("2. Sort and display the employee records");
            Console.WriteLine("3. Search for an employee");
            Console.WriteLine("4. Save into text file");
            Console.WriteLine("0. Exit");
            Console.WriteLine("\n\n");
            Console.WriteLine(printTitle("Please select an option"));//Print title to make user enter the input
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
                employees.Add(new Employee(int.Parse(employee[0]), employee[1], employee[2], double.Parse(employee[3]), float.Parse(employee[4])));//Store the values in the employee
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
                    printPayroll();//Calculate payroll
                    break;
                case ConsoleKey.D2://Key is 2
                case ConsoleKey.NumPad2://Key is num2
                    sortEmployees();//Sort the emplyees
                    printEmployee();//Print the employees
                    break;
                case ConsoleKey.D3://Key is 3
                case ConsoleKey.NumPad3://Key is num3
                    searchEmployee();//Search the employee
                    break;
                case ConsoleKey.D4://Key is 4
                case ConsoleKey.NumPad4://Key is num4
                    saveData();//Save data to file
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
        public void calculateFortnight()
        {
            for (int i = 0; i < employees.Count; i++)//Set payrolls for the all employees
                setPayroll(i);
        }

        //Print payroll of searched employee
        public void printPayroll()
        {
            //Print Employee
            printEmployee();
            //Print Title
            Console.WriteLine(printTitle("ENTER ID"));
            //Search for the employee against his ID
            Employee? employee = search(int.Parse(Console.ReadLine()));
            //Clear the console
            Console.Clear();
            //If employee was found i.e. not null
            //Then print employee and his payroll
            if (employee != null)
            {
                Console.WriteLine(((Employee)employee).print());//Print employees
                Console.WriteLine(((Employee)employee).payroll.print());//Print their payroll
            }
            else
                //Otherwise print invalid id
                Console.WriteLine("INVALID ID!");
            //If user pressed s then saved other wise returns to menu
            Console.WriteLine("\nPRESS ANY KEY TO CONTINUE OR S TO SAVE!");
            if (Console.ReadKey().Key == ConsoleKey.S)//If key pressed is S
                saveData();//Save the data
        }

        //Print all employees in form of a table
        public void printEmployee()
        {
            Console.WriteLine(printTitle("EMPLOYEES"));//Print employees title
            Console.WriteLine("\n");//Print next line
            Console.WriteLine(new String('-', 97));//Print repeated string'-' 97 times
            Console.WriteLine("|Employee ID\t|Employee First Name\t|Employee Last Name\t|Annual Income\t|KiwiSaver\t|");//Print table title
            Console.WriteLine(new String('-', 97));//Print repeated string'-' 97 times
            foreach (Employee employee in employees)//For each employee in the employees
            {
                Console.WriteLine(employee.toString());//Write each employee
                Console.WriteLine(new String('-', 97));//Print repeated string'-' 97 times
            }
            Console.WriteLine("\n");//print next line
        }

        //Sort the employees
        public void sortEmployees()
        {
            employees.Sort(new compare());//Sort employees by comparing
        }

        //Seach for employee and print it
        public void searchEmployee()
        {
            Console.WriteLine("Please enter the id of employee : ");//print instruction
            Employee? employee = search(int.Parse(Console.ReadLine()));//Search for the employee at read id
            Console.Clear();//Clear the console
            if (employee != null)//If employee was null
            {
                Console.WriteLine(new String('-', 97));//Print repeated string'-' 97 times
                Console.WriteLine("|Employee ID\t|Employee First Name\t|Employee Last Name\t|Annual Income\t|KiwiSaver\t|");//Print table title
                Console.WriteLine(new String('-', 97));//Print repeated string'-' 97 times
                Console.WriteLine(((Employee)employee).toString());//Print employee
                Console.WriteLine(new String('-', 97));//Print repeated string'-' 97 times
            }
            else
                Console.WriteLine("Invalid Id!");//Else the id was invalid
        }

        //Search for an employee against id and return it
        public Employee? search(int id)
        {
            foreach (Employee e in employees)//For each employee in the employees
                if (e.id == id)//If id matches
                    return e;//Return the employee
            return null;//Else return null
        }

        /*
         * Save the employee in weekly_payroll.txt and file
         * modes are OpenOrCreate and write.
         * After writing close the file
         */
        public void saveData()
        {
            FileStream fileStream = new FileStream("weekly_payroll.txt", FileMode.OpenOrCreate, FileAccess.Write);//Create file reader with open, create and write
            StreamWriter streamWriter = new StreamWriter(fileStream);//Create file writer
            foreach (Employee employee in employees)//For employee in employees
            {
                streamWriter.Write(employee.print() + "\n");//Write employee in the file
                streamWriter.Write(employee.payroll.print() + "\n");//Write payroll in the file
            }
            streamWriter.Close();//Close the writer
            fileStream.Close();//Close the file
            Console.Clear();//Clear the console
            Console.WriteLine("Saved Successfully!");//Print message
        }

        //Print the title
        public static string printTitle(string title)
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
        public void setPayroll(int index)
        {
            Employee employee = employees[index];//Employee at index
            double hourlyRate = (employee.income / 52 / 40);//Hourly rate of employee
            double grossFortnightSalary = hourlyRate * 80;//2 week salary
            double kiwiSaver = grossFortnightSalary * employee.kiwiSaver / 100;//Calcualte kivi saver value
            double tax = 0;//Tax variable
            int hoursWorked = 80;//Hours worked
            //Calculate tax value
            if (employee.income <= 14000)
                tax = grossFortnightSalary * 10.5 / 100;
            else if (employee.income <= 48000)
                tax = grossFortnightSalary * 17.5 / 100;
            else if (employee.income <= 70000)
                tax = grossFortnightSalary * 30.0 / 100;
            else if (employee.income <= 180000)
                tax = grossFortnightSalary * 33.0 / 100;
            else
                tax = employee.income * 39 / 100;
            double fortnightPay = grossFortnightSalary - kiwiSaver - tax;//Calcualte fortnite net pay
            employee.payroll = new payroll(hourlyRate: (int)Math.Round(hourlyRate), hoursWorked: hoursWorked, grossPay: (int)Math.Round(grossFortnightSalary), (int)Math.Round(fortnightPay), (int)Math.Round(tax), (int)Math.Round(kiwiSaver));//Set employee payroll
            employees[index] = employee;//Set the new employee
        }
    }

    //Structure Employee
    struct Employee
    {
        //Properties
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public double income { get; set; }
        public float kiwiSaver { get; set; }

        public payroll payroll { get; set; }

        //Constructor
        public Employee(int id, string firstName, string lastName, double income, float kiwiSaver)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.income = income;
            this.kiwiSaver = kiwiSaver;
            payroll = null;
        }
        //To String
        public string toString()
        {
            return String.Format("|{0,-11}\t|{1,-19}\t|{2,-18}\t|{3,-13}\t|{4,-15}|", id, firstName, lastName, income, kiwiSaver + "%");//String in the format 
        }

        //Print values
        public string print()
        {
            return
                Program.printTitle("EMPLOYEE DETAILS") + "\n" +
                "Employee ID : " + this.id + "\n" +
                "First Name : " + this.firstName + "\n" +
                "Last Name : " + this.lastName + "\n" +
                "Yearly Income : " + this.income + "\n" +
                "Kiwi Saver : " + this.kiwiSaver + "%" + "\n";
        }
    }

    //Payroll class
    class payroll
    {
        //Properties
        public int hourlyRate { get; set; }
        public int hoursWorked { get; set; }
        public float grossPay { get; set; }
        public float fortnightPayroll { get; set; }

        public float tax { get; set; }
        public float kiwiSaver { get; set; }

        //Constructors
        public payroll(int hourlyRate, int hoursWorked, float grossPay, float fortnightPayroll, float tax, float kiwiSaver)
        {
            this.hourlyRate = hourlyRate;
            this.hoursWorked = hoursWorked;
            this.grossPay = grossPay;
            this.fortnightPayroll = fortnightPayroll;
            this.tax = tax;
            this.kiwiSaver = kiwiSaver;
        }

        //print payroll
        public string print()
        {
            return
                Program.printTitle("PAYROLL") + "\n" +
                "Hourly Rate : " + this.hourlyRate + "$" + "\n" +
                "Hours worked : " + this.hoursWorked + "hr" + "\n" +
                "Gross Pay : " + this.grossPay + "$" + "\n" +
                "DEDUCTIONS" + "\n" +
                "Tax : " + this.tax + "$" + "\n" +
                "Kiwi Saver : " + this.kiwiSaver + "$" + "\n" +
                Program.printTitle("PAY") + "\n" +
                "Fortnight Payroll : " + this.fortnightPayroll + "$" + "\n";
        }
    }

    class compare : IComparer<Employee>
    {
        public int Compare(Employee x, Employee y)
        {
            return x.id.CompareTo(y.id);
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
