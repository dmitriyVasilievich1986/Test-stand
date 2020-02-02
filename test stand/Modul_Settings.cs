using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Support_Class;

namespace test_stand
{
    public partial class Modul_Settings : Form
    {
        Module_Parameters MP;

        public Modul_Settings(Module_Parameters module_parameters)
        {
            InitializeComponent();
            MP = module_parameters;
            Din.Text = $"{MP.using_module.din.Min} {MP.using_module.din.Max} {MP.using_module.din.None}";
            KF.Text = $"{MP.using_module.kf.Min} {MP.using_module.kf.Max} {MP.using_module.kf.None}";
            TC.Text = $"{MP.using_module.tc.Min} {MP.using_module.tc.Max} {MP.using_module.tc.None}";
            TC12V.Text = $"{MP.using_module.tc12v.Min} {MP.using_module.tc12v.Max} {MP.using_module.tc12v.None}";
            Current.Text = $"{MP.using_module.current}";
            
        }

        private void Parameters_Change(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
                int name = Convert.ToInt32((((TextBox)sender).Name).Replace("Param", string.Empty));
            using (SqlConnection connection = new SqlConnection(Data_Transit.connectionString))
            {
                string sqlExpression = "";
                connection.Open();                
                SqlParameter nameParam = new SqlParameter("@name", Data_Transit.Name);
                SqlParameter ColumnParam = new SqlParameter();
                SqlParameter NumberParam = new SqlParameter();
                string NewParameters = ((TextBox)sender).Text;
                switch (name)
                {
                    case 1:
                        Data_Transit.Module_Parameters["din"][0] = Convert.ToInt32(NewParameters);
                        sqlExpression = "UPDATE [user].[dbo].[module_parameters] SET dinmin=@number where module=@name";
                        NumberParam = new SqlParameter("@number", Convert.ToInt32(NewParameters));
                        break;
                    case 2:
                        Data_Transit.Module_Parameters["din"][1] = Convert.ToInt32(NewParameters);
                        sqlExpression = "UPDATE [user].[dbo].[module_parameters] SET dinmax=@number where module=@name";
                        NumberParam = new SqlParameter("@number", Convert.ToInt32(NewParameters));
                        break;
                    case 3:
                        Data_Transit.Module_Parameters["din"][2] = Convert.ToInt32(NewParameters);
                        sqlExpression = "UPDATE [user].[dbo].[module_parameters] SET dinnone=@number where module=@name";
                        NumberParam = new SqlParameter("@number", Convert.ToInt32(NewParameters));
                        break;
                    case 4:
                        Data_Transit.Module_Parameters["kf"][0] = Convert.ToInt32(NewParameters);
                        sqlExpression = "UPDATE [user].[dbo].[module_parameters] SET kfmin=@number where module=@name";
                        NumberParam = new SqlParameter("@number", Convert.ToInt32(NewParameters));
                        break;
                    case 5:
                        Data_Transit.Module_Parameters["kf"][1] = Convert.ToInt32(NewParameters);
                        sqlExpression = "UPDATE [user].[dbo].[module_parameters] SET kfmax=@number where module=@name";
                        NumberParam = new SqlParameter("@number", Convert.ToInt32(NewParameters));
                        break;
                    case 6:
                        Data_Transit.Module_Parameters["kf"][2] = Convert.ToInt32(NewParameters);
                        sqlExpression = "UPDATE [user].[dbo].[module_parameters] SET kfnone=@number where module=@name";
                        NumberParam = new SqlParameter("@number", Convert.ToInt32(NewParameters));
                        break;
                    case 7:
                        Data_Transit.Module_Parameters["tc"][0] = Convert.ToInt32(NewParameters);
                        sqlExpression = "UPDATE [user].[dbo].[module_parameters] SET tcmin=@number where module=@name";
                        NumberParam = new SqlParameter("@number", Convert.ToInt32(NewParameters));
                        break;
                    case 8:
                        Data_Transit.Module_Parameters["tc"][1] = Convert.ToInt32(NewParameters);
                        sqlExpression = "UPDATE [user].[dbo].[module_parameters] SET tcmax=@number where module=@name";
                        NumberParam = new SqlParameter("@number", Convert.ToInt32(NewParameters));
                        break;
                    case 9:
                        Data_Transit.Module_Parameters["tc"][2] = Convert.ToInt32(NewParameters);
                        sqlExpression = "UPDATE [user].[dbo].[module_parameters] SET tcnone=@number where module=@name";
                        NumberParam = new SqlParameter("@number", Convert.ToInt32(NewParameters));
                        break;
                    case 10:
                        Data_Transit.Current_Norm = Convert.ToSingle(NewParameters);
                        sqlExpression = "UPDATE [user].[dbo].[module_parameters] SET [current]=@number where module=@name";
                        NumberParam = new SqlParameter("@number", Convert.ToSingle(NewParameters));
                        break;
                    case 11:
                        Data_Transit.module.Addres = Convert.ToByte(Param11.Text);
                        foreach(string rec in Data_Transit.Registers_Module.Keys) Data_Transit.Registers_Module[rec][0] = Data_Transit.module.Addres;
                        return;
                    case 12:
                        Data_Transit.TC_12V = int.Parse(TC12V.Text);
                        return;
                    case 13:
                        Data_Transit.exchange_port = int.Parse(Param13.Text);
                        return;
                    case 14:
                        Data_Transit.port = int.Parse(Param14.Text);
                        return;
                }
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.Add(NumberParam);
                command.Parameters.Add(nameParam);
                command.ExecuteNonQuery();
            }
        }
    }
}
