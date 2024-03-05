using PhoneBook;
class Program
{
    static void Main(String[] h)
    {
        Console.WriteLine("Started");
        //Console.WriteLine(DAO.createTable());
        //Console.WriteLine(DAO.insert());
        Console.WriteLine("----------------1st------------");
       // DAO.selectAll();
       //Console.WriteLine(DAO.delete());
        Console.WriteLine("--------------2nd-----------");
        // DAO.selectAll();
        DAO.update();
        Console.WriteLine("end");
    }
}