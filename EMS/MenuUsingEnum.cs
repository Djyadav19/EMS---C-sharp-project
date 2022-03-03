using System;

namespace EMS;

class MenuUsingEnum
{
    enum AdminMenu
    {
        To_add_an_Employee = 1,
        To_Manipulate_Employee_Details = 2,
        To_Retrieve_Employee_Details = 3,
        To_Delete_Employee_Details = 4,
        To_generate_Payroll = 5,
        To_Logout= 6,
    }
    /*
    enum LoginPageMenu
    {
        Login = 1,
        To_Read_how_to_register_with_us = 2,
        Exit = 3,
    }

    enum EmployeeMenu
    {
        To_display_yours_details = 1,
        To_generate_your_payroll = 2,
    }*/
    public static void AdminOptions()
    {
        for (int i = 1; i <= (int)Enum.GetNames(typeof(AdminMenu)).Length; i++)
        {
            Console.WriteLine($"[{i}] {((AdminMenu)i).ToString().Replace("_", " ")}" + " : ");
        }
    }
}