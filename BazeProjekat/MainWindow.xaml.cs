using Baze;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
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

namespace BazeProjekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string current;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void osoba_Click(object sender, RoutedEventArgs e)
        {
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            current = "osoba";
            if (osoba.Content.Equals("Dodaj/azuriraj"))
            {
                handlujAddOsobe();
                return;
            }
            resetujDugmice();
            izlistaj();
            prikaziUIOsobe();
            osoba.Content = "Dodaj/azuriraj";
            osoba.Foreground = new SolidColorBrush(Colors.Red);
        }
        private void handlujAddOsobe()
        {
            var context = new BazeProjekatEntities();
            Osoba o = new Osoba();
            bool update = false;
            foreach (object item in kanvas.Children)
            {
                if (!(item is TextBox))
                    continue;

                if ((item as TextBox).Name.Equals("jmbg"))
                {
                    if ((item as TextBox).Text.Equals(string.Empty) || (item as TextBox).Text.Equals("KEY"))
                    {
                        poruka.Text = "Unesite redovan kljuc!";
                        return;
                    }
                    object osoba = context.Osoba.ToList().Find(x => x.JMBG.Equals((item as TextBox).Text));
                    if (osoba == null)
                    {
                        o.JMBG = (item as TextBox).Text;
                    }
                    else
                    {
                        o = osoba as Osoba;
                        update = true;
                    }
                }
                if ((item as TextBox).Name.Equals("imprz"))
                {
                    try
                    {
                        string s = (item as TextBox).Text.Split('_')[1];
                    }
                    catch
                    {
                        poruka.Text = "Ime i prezime nisu u dobrom formatu!";
                        return;
                    }
                    if (update)
                    {
                        
                        if (o.ImPrz.Equals((item as TextBox).Text))
                        {
                            poruka.Text = "Entitet je ostao isti";
                            return;
                        }

                        context.Osoba.ToList().Find(x => x.JMBG.Equals(o.JMBG)).ImPrz = (item as TextBox).Text;
                        if(context.Radnik.ToList().Find(x => x.JMBG.Equals(o.JMBG)) != null)
                        {
                            context.Radnik.ToList().Find(x => x.JMBG.Equals(o.JMBG)).ImPrz = (item as TextBox).Text;
                            if (context.Pilot.ToList().Find(x => x.JMBG.Equals(o.JMBG)) != null)
                            {
                                context.Pilot.ToList().Find(x => x.JMBG.Equals(o.JMBG)).ImPrz = (item as TextBox).Text;
                            }
                        }
                        if (context.Putnik.ToList().Find(x => x.JMBG.Equals(o.JMBG)) != null)
                        {
                            context.Putnik.ToList().Find(x => x.JMBG.Equals(o.JMBG)).ImPrz = (item as TextBox).Text;
                        }
                    }
                    else
                    {
                        o.ImPrz = (item as TextBox).Text;
                        context.Osoba.Add(o);
                    }
                }
            }
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            context.SaveChanges();
            izlistaj();
        }
        private void prikaziUIOsobe()
        {
            TextBlock tbljmbg = new TextBlock();
            tbljmbg.Text = "JMBG:";
            tbljmbg.Margin = new Thickness() { Left = 610, Top = 175, Right = 0, Bottom = 0 };

            TextBox tbJMBG = new TextBox();
            tbJMBG.Name = "jmbg";
            tbJMBG.Text = "KEY";
            tbJMBG.Width = 100;
            tbJMBG.Height = 20;
            tbJMBG.Margin = new Thickness() { Left = 650, Top = 175, Right = 0, Bottom = 0 };

            TextBlock imprztbl = new TextBlock();
            imprztbl.Text = "imprz:";
            imprztbl.Margin = new Thickness() { Left = 610, Top = 210, Right = 0, Bottom = 0 };

            TextBox IMPRZtb = new TextBox();
            IMPRZtb.Name = "imprz";
            IMPRZtb.Text = "Ime_Prezime";
            IMPRZtb.Width = 100;
            IMPRZtb.Height = 20;
            IMPRZtb.Margin = new Thickness() { Left = 650, Top = 210, Right = 0, Bottom = 0 };

            kanvas.Children.Add(tbJMBG);
            kanvas.Children.Add(tbljmbg);
            kanvas.Children.Add(imprztbl);
            kanvas.Children.Add(IMPRZtb);
        }

        private void putnik_Click(object sender, RoutedEventArgs e)
        {
            poruka.Text = "Rukovanje putnicima se radi kroz osobe";
            current = "putnik";
            if (putnik.Content.Equals("Dodaj/azuriraj"))
            {
                handlujAddPutnik();
                return;
            }
            resetujDugmice();
            izlistaj();
            prikaziUIPutnik();
            putnik.Content = "Dodaj/azuriraj";
            putnik.Foreground = new SolidColorBrush(Colors.Red);
        }
        private void prikaziUIPutnik()
        {
            TextBlock tbljmbg = new TextBlock();
            tbljmbg.Text = "JMBG:";
            tbljmbg.Margin = new Thickness() { Left = 610, Top = 175, Right = 0, Bottom = 0 };

            ComboBox cb = new ComboBox();
            var context = new BazeProjekatEntities();
            cb.Margin = new Thickness() { Left = 650, Top = 175, Right = 0, Bottom = 0 };
            cb.Width = 100;
            cb.Height = 20;
            cb.Items.Add("-");
            foreach (Osoba o in context.Osoba)
            {
                if (context.Radnik.ToList().Find(x => x.JMBG.Equals(o.JMBG)) == null && context.Putnik.ToList().Find(x => x.JMBG.Equals(o.JMBG)) == null)
                {
                    cb.Items.Add(o.JMBG);
                }
            } 
            (cb as ComboBox).SelectedIndex = 0;
            kanvas.Children.Add(tbljmbg);
            kanvas.Children.Add(cb);
        }
        private void handlujAddPutnik()
        {
            var context = new BazeProjekatEntities();
            Putnik o = new Putnik();
            foreach (object item in kanvas.Children)
            {
                if (!(item is ComboBox))
                    continue;

                
                if ((item as ComboBox).SelectedItem.Equals("-"))
                {
                    return;
                }

                o.JMBG = context.Osoba.ToList().Find(x => x.JMBG.Equals((item as ComboBox).SelectedItem)).JMBG;
                o.ImPrz = context.Osoba.ToList().Find(x => x.JMBG.Equals((item as ComboBox).SelectedItem)).ImPrz;

                context.Putnik.Add(o);
                (item as ComboBox).Items.Remove((item as ComboBox).SelectedItem);
                (item as ComboBox).SelectedItem = (item as ComboBox).Items[0];
            }
            poruka.Text = "Rukovanje putnicima se radi kroz osobe";
            context.SaveChanges();
            izlistaj();
        }
        
        private void pista_Click(object sender, RoutedEventArgs e)
        {
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            current = "pista";
            if (pista.Content.Equals("Dodaj/azuriraj"))
            {
                handlujAddPista();
                return;
            }
            resetujDugmice();
            izlistaj();
            prikaziUIPista();
            pista.Content = "Dodaj/azuriraj";
            pista.Foreground = new SolidColorBrush(Colors.Red);
        }
        private void prikaziUIPista()
        {
            TextBlock tbljmbg = new TextBlock();
            tbljmbg.Text = "PID:";
            tbljmbg.Margin = new Thickness() { Left = 610, Top = 175, Right = 0, Bottom = 0 };

            TextBox tbJMBG = new TextBox();
            tbJMBG.Name = "jmbg";
            tbJMBG.Text = "KEY";
            tbJMBG.Width = 100;
            tbJMBG.Height = 20;
            tbJMBG.Margin = new Thickness() { Left = 650, Top = 175, Right = 0, Bottom = 0 };

            TextBlock imprztbl = new TextBlock();
            imprztbl.Text = "kula:";
            imprztbl.Margin = new Thickness() { Left = 610, Top = 210, Right = 0, Bottom = 0 };

            TextBox IMPRZtb = new TextBox();
            IMPRZtb.Name = "imprz";
            IMPRZtb.Text = "nazivKule";
            IMPRZtb.Width = 100;
            IMPRZtb.Height = 20;
            IMPRZtb.Margin = new Thickness() { Left = 650, Top = 210, Right = 0, Bottom = 0 };

            kanvas.Children.Add(tbJMBG);
            kanvas.Children.Add(tbljmbg);
            kanvas.Children.Add(imprztbl);
            kanvas.Children.Add(IMPRZtb);
        }
        private void handlujAddPista()
        {
            var context = new BazeProjekatEntities();
            Pista o = new Pista();
            bool update = false;
            foreach (object item in kanvas.Children)
            {
                if (!(item is TextBox))
                    continue;

                if ((item as TextBox).Name.Equals("jmbg"))
                {
                    if ((item as TextBox).Text.Equals(string.Empty) || (item as TextBox).Text.Equals("KEY"))
                    {
                        poruka.Text = "Unesite redovan kljuc!";
                        return;
                    }
                    object osoba = context.Pista.ToList().Find(x => x.PID.Equals((item as TextBox).Text));
                    if (osoba == null)
                    {
                        o.PID = (item as TextBox).Text;
                    }
                    else
                    {
                        o = osoba as Pista;
                        update = true;
                    }
                }
                if ((item as TextBox).Name.Equals("imprz"))
                {
                    if (update)
                    {
                        object osoba = context.Pista.ToList().Find(x => x.PID.Equals(o.PID));
                        
                        o.Kula = (item as TextBox).Text;

                        context.Pista.ToList().Find(x => x.Equals(osoba)).Kula = o.Kula;
                    }
                    else
                    {
                        o.Kula = (item as TextBox).Text;
                        context.Pista.Add(o);
                    }
                }
            }
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            context.SaveChanges();
            izlistaj();
        }

        private void radnik_Click(object sender, RoutedEventArgs e)
        {
            poruka.Text = "Rukovanje radnicima se radi kroz osobe";
            current = "radnik";
            if (radnik.Content.Equals("Dodaj/azuriraj"))
            {
                handlujAddRadnik();
                return;
            }
            resetujDugmice();
            izlistaj();
            prikaziUIRadnik();
            radnik.Content = "Dodaj/azuriraj";
            radnik.Foreground = new SolidColorBrush(Colors.Red);
        }
        private void prikaziUIRadnik()
        {
            TextBlock tbljmbg = new TextBlock();
            tbljmbg.Text = "JMBG:";
            tbljmbg.Margin = new Thickness() { Left = 610, Top = 175, Right = 0, Bottom = 0 };

            ComboBox cb = new ComboBox();
            var context = new BazeProjekatEntities();
            cb.Margin = new Thickness() { Left = 650, Top = 175, Right = 0, Bottom = 0 };
            cb.Width = 100;
            cb.Height = 20;
            cb.Items.Add("-");
            foreach (Osoba o in context.Osoba)
            {
                if (context.Putnik.ToList().Find(x => x.JMBG.Equals(o.JMBG)) == null && context.Radnik.ToList().Find(x => x.JMBG.Equals(o.JMBG)) == null)
                {
                    cb.Items.Add(o.JMBG);
                }
            }
            (cb as ComboBox).SelectedIndex = 0;
            kanvas.Children.Add(tbljmbg);
            kanvas.Children.Add(cb);
        }
        private void handlujAddRadnik()
        {
            var context = new BazeProjekatEntities();
            Radnik o = new Radnik();
            foreach (object item in kanvas.Children)
            {
                if (!(item is ComboBox))
                    continue;

                if ((item as ComboBox).SelectedItem.Equals("-"))
                {
                    return;
                }

                o.JMBG = context.Osoba.ToList().Find(x => x.JMBG.Equals((item as ComboBox).SelectedItem)).JMBG;
                o.ImPrz = context.Osoba.ToList().Find(x => x.JMBG.Equals((item as ComboBox).SelectedItem)).ImPrz;

                context.Radnik.Add(o);
                (item as ComboBox).Items.Remove((item as ComboBox).SelectedItem);
                (item as ComboBox).SelectedItem = (item as ComboBox).Items[0];
            }
            poruka.Text = "Rukovanje radnicima se radi kroz osobe";
            try
            {
                context.SaveChanges();
            }
            catch(Exception e)
            {
                poruka.Text = e.InnerException.InnerException.Message;
            }
            izlistaj();
        }

        private void obavestenje_Click(object sender, RoutedEventArgs e)
        {
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            current = "obavestenje";
            if (obavestenje.Content.Equals("Dodaj/azuriraj"))
            {
                handlujAddObavestenje();
                return;
            }
            resetujDugmice();
            izlistaj();
            prikaziUIObavestenje();
            obavestenje.Content = "Dodaj/azuriraj";
            obavestenje.Foreground = new SolidColorBrush(Colors.Red);
        }
        private void prikaziUIObavestenje()
        {
            TextBlock tbljmbg = new TextBlock();
            tbljmbg.Text = "OID:";
            tbljmbg.Margin = new Thickness() { Left = 610, Top = 175, Right = 0, Bottom = 0 };

            TextBox tbJMBG = new TextBox();
            tbJMBG.Name = "jmbg";
            tbJMBG.Text = "KEY";
            tbJMBG.Width = 100;
            tbJMBG.Height = 20;
            tbJMBG.Margin = new Thickness() { Left = 650, Top = 175, Right = 0, Bottom = 0 };

            TextBlock imprztbl = new TextBlock();
            imprztbl.Text = "Poruka:";
            imprztbl.Margin = new Thickness() { Left = 610, Top = 245, Right = 0, Bottom = 0 };

            TextBox IMPRZtb = new TextBox();
            IMPRZtb.Name = "imprz";
            IMPRZtb.Text = "Poruka";
            IMPRZtb.Width = 100;
            IMPRZtb.Height = 20;
            IMPRZtb.Margin = new Thickness() { Left = 650, Top = 245, Right = 0, Bottom = 0 };

            TextBlock tbl = new TextBlock();
            tbl.Text = "Radnik:";
            tbl.Margin = new Thickness() { Left = 610, Top = 210, Right = 0, Bottom = 0 };

            ComboBox cb = new ComboBox();
            var context = new BazeProjekatEntities();
            cb.Margin = new Thickness() { Left = 650, Top = 210, Right = 0, Bottom = 0 };
            cb.Width = 100;
            cb.Height = 20;
            cb.Items.Add("-");
            foreach (Radnik o in context.Radnik)
            {
                cb.Items.Add(o.JMBG);
            }

            kanvas.Children.Add(tbl);
            kanvas.Children.Add(cb);
            kanvas.Children.Add(tbJMBG);
            kanvas.Children.Add(tbljmbg);
            kanvas.Children.Add(imprztbl);
            kanvas.Children.Add(IMPRZtb);
            (cb as ComboBox).SelectedIndex = 0;
        }
        private void handlujAddObavestenje()
        {
            var context = new BazeProjekatEntities();
            Obavestenje o = new Obavestenje();
            bool update = false;
            foreach (object item in kanvas.Children)
            {
                if(item is ComboBox)
                {
                    if ((item as ComboBox).SelectedIndex == 0)
                    {
                        poruka.Text = "Izaberi radnika";
                        return;
                    }
                    o.JMBG_FK = (item as ComboBox).SelectedItem.ToString();
                    o.Radnik = context.Radnik.ToList().Find(x => x.JMBG.Equals((item as ComboBox).SelectedItem));
                }
                if (!(item is TextBox))
                    continue;

                if ((item as TextBox).Name.Equals("jmbg"))
                {
                    if ((item as TextBox).Text.Equals(string.Empty) || (item as TextBox).Text.Equals("KEY"))
                    {
                        poruka.Text = "Unesite redovan kljuc!";
                        return;
                    }
                    object osoba = context.Obavestenje.ToList().Find(x => x.OID.Equals((item as TextBox).Text));
                    if (osoba == null)
                    {
                        o.OID = (item as TextBox).Text;
                    }
                    else
                    {
                        o = osoba as Obavestenje;
                        update = true;
                    }
                }
                if ((item as TextBox).Name.Equals("imprz"))
                {
                    if (update)
                    {
                        object osoba = context.Obavestenje.ToList().Find(x => x.OID.Equals(o.OID));
                        
                        o.Poruka = (item as TextBox).Text;

                        context.Obavestenje.ToList().Find(x => x.Equals(osoba)).Poruka = o.Poruka;
                        context.Obavestenje.ToList().Find(x => x.Equals(osoba)).JMBG_FK = o.JMBG_FK;
                        context.Obavestenje.ToList().Find(x => x.Equals(osoba)).Radnik = o.Radnik;

                    }
                    else
                    {
                        o.Poruka = (item as TextBox).Text;
                        context.Obavestenje.Add(o);
                    }
                }
            }
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            context.SaveChanges();
            izlistaj();
        }

        private void karta_Click(object sender, RoutedEventArgs e)
        {
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            current = "karta";
            if (karta.Content.Equals("Dodaj/azuriraj"))
            {
                handlujAddKarta();
                return;
            }
            resetujDugmice();
            izlistaj();
            prikaziUIKarta();
            karta.Content = "Dodaj/azuriraj";
            karta.Foreground = new SolidColorBrush(Colors.Red);
        }
        private void prikaziUIKarta()
        {
            TextBlock tbljmbg = new TextBlock();
            tbljmbg.Text = "KID:";
            tbljmbg.Margin = new Thickness() { Left = 610, Top = 175, Right = 0, Bottom = 0 };

            TextBox tbJMBG = new TextBox();
            tbJMBG.Name = "jmbg";
            tbJMBG.Text = "KEY";
            tbJMBG.Width = 100;
            tbJMBG.Height = 20;
            tbJMBG.Margin = new Thickness() { Left = 650, Top = 175, Right = 0, Bottom = 0 };

            TextBlock imprztbl = new TextBlock();
            imprztbl.Text = "Klasa:";
            imprztbl.Margin = new Thickness() { Left = 610, Top = 210, Right = 0, Bottom = 0 };

            TextBox IMPRZtb = new TextBox();
            IMPRZtb.Name = "imprz";
            IMPRZtb.Text = "Klasa";
            IMPRZtb.Width = 100;
            IMPRZtb.Height = 20;
            IMPRZtb.Margin = new Thickness() { Left = 650, Top = 210, Right = 0, Bottom = 0 };

            TextBlock tbl = new TextBlock();
            tbl.Text = "Putnik:";
            tbl.Margin = new Thickness() { Left = 610, Top = 245, Right = 0, Bottom = 0 };

            ComboBox cb = new ComboBox();
            var context = new BazeProjekatEntities();
            cb.Margin = new Thickness() { Left = 650, Top = 245, Right = 0, Bottom = 0 };
            cb.Width = 100;
            cb.Height = 20;
            cb.Name = "putnik";
            cb.Items.Add("-");
            foreach (Putnik o in context.Putnik)
            {
                cb.Items.Add(o.JMBG);
            }

            TextBlock tbl1 = new TextBlock();
            tbl1.Text = "Let:";
            tbl1.Margin = new Thickness() { Left = 610, Top = 280, Right = 0, Bottom = 0 };

            ComboBox cb1 = new ComboBox();
            cb1.Margin = new Thickness() { Left = 650, Top = 280, Right = 0, Bottom = 0 };
            cb1.Width = 100;
            cb1.Height = 20;
            cb1.Items.Add("-");
            foreach (Let o in context.Let)
            {
                cb1.Items.Add(o.LID);
            }

            kanvas.Children.Add(tbl);
            kanvas.Children.Add(cb);
            kanvas.Children.Add(tbl1);
            kanvas.Children.Add(cb1);
            kanvas.Children.Add(tbJMBG);
            kanvas.Children.Add(tbljmbg);
            kanvas.Children.Add(imprztbl);
            kanvas.Children.Add(IMPRZtb);
            (cb as ComboBox).SelectedIndex = 0;
            (cb1 as ComboBox).SelectedIndex = 0;
        }
        private void handlujAddKarta()
        {
            var context = new BazeProjekatEntities();
            Karta o = new Karta();
            bool update = false;
            foreach (object item in kanvas.Children)
            {
                if (item is ComboBox)
                {
                    if ((item as ComboBox).Name.Equals("putnik"))
                    {
                        if((item as ComboBox).SelectedIndex == 0)
                        {
                            continue;
                        } 
                        o.JMBG_FK = (item as ComboBox).SelectedItem.ToString();
                        o.Putnik = context.Putnik.ToList().Find(x => x.JMBG.Equals((item as ComboBox).SelectedItem));
                    }
                    else
                    {
                        if ((item as ComboBox).SelectedIndex == 0)
                        {
                            poruka.Text = "Izaberi let";
                            return;
                        }
                        o.LID_FK = (item as ComboBox).SelectedItem.ToString();
                        o.Let = context.Let.ToList().Find(x => x.LID.Equals((item as ComboBox).SelectedItem));
                    }

                }
                if (!(item is TextBox))
                    continue;

                if ((item as TextBox).Name.Equals("jmbg"))
                {
                    if ((item as TextBox).Text.Equals(string.Empty) || (item as TextBox).Text.Equals("KEY"))
                    {
                        poruka.Text = "Unesite redovan kljuc!";
                        return;
                    }
                    object osoba = context.Karta.ToList().Find(x => x.KID.Equals((item as TextBox).Text));
                    if (osoba == null)
                    {
                        o.KID = (item as TextBox).Text;
                    }
                    else
                    {
                        o.KID = (osoba as Karta).KID;
                        o.Klasa = (osoba as Karta).Klasa;
                        update = true;
                    }
                }
                if ((item as TextBox).Name.Equals("imprz"))
                {
                    if (update)
                    {
                        object osoba = context.Karta.ToList().Find(x => x.KID.Equals(o.KID));

                        o.Klasa = (item as TextBox).Text;

                        context.Karta.ToList().Find(x => x.Equals(osoba)).JMBG_FK = o.JMBG_FK;
                        context.Karta.ToList().Find(x => x.Equals(osoba)).Putnik = o.Putnik;
                        context.Karta.ToList().Find(x => x.Equals(osoba)).Klasa = o.Klasa;
                        context.Karta.ToList().Find(x => x.Equals(osoba)).LID_FK = o.LID_FK;
                        context.Karta.ToList().Find(x => x.Equals(osoba)).Let = o.Let;

                    }
                    else
                    {
                        o.Klasa = (item as TextBox).Text;
                        context.Karta.Add(o);
                    }
                }
            }
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            context.SaveChanges();
            izlistaj();
        }

        private void pilot_Click(object sender, RoutedEventArgs e)
        {
            poruka.Text = "Rukovanje pilotima se radi kroz osobe";
            current = "pilot";
            if (pilot.Content.Equals("Dodaj/azuriraj"))
            {
                handlujAddPilot();
                return;
            }
            resetujDugmice();
            izlistaj();
            prikaziUIPilot();
            pilot.Content = "Dodaj/azuriraj";
            pilot.Foreground = new SolidColorBrush(Colors.Red);
        }
        private void prikaziUIPilot()
        {
            TextBlock tbljmbg = new TextBlock();
            tbljmbg.Text = "JMBG:";
            tbljmbg.Margin = new Thickness() { Left = 610, Top = 175, Right = 0, Bottom = 0 };

            ComboBox cb = new ComboBox();
            var context = new BazeProjekatEntities();
            cb.Margin = new Thickness() { Left = 650, Top = 175, Right = 0, Bottom = 0 };
            cb.Width = 100;
            cb.Height = 20;
            cb.Items.Add("-");
            foreach (Radnik o in context.Radnik)
            {
                if (context.Pilot.ToList().Find(x => x.JMBG.Equals(o.JMBG)) == null)
                {
                    cb.Items.Add(o.JMBG);
                }
            }
            (cb as ComboBox).SelectedIndex = 0;
            kanvas.Children.Add(tbljmbg);
            kanvas.Children.Add(cb);
        }
        private void handlujAddPilot()
        {
            var context = new BazeProjekatEntities();
            Pilot o = new Pilot();
            foreach (object item in kanvas.Children)
            {
                if (!(item is ComboBox))
                    continue;

                if ((item as ComboBox).SelectedItem.Equals("-"))
                {
                    return;
                }

                o.JMBG = context.Radnik.ToList().Find(x => x.JMBG.Equals((item as ComboBox).SelectedItem)).JMBG;
                o.ImPrz = context.Radnik.ToList().Find(x => x.JMBG.Equals((item as ComboBox).SelectedItem)).ImPrz;

                context.Pilot.Add(o);
                (item as ComboBox).Items.Remove((item as ComboBox).SelectedItem);
                (item as ComboBox).SelectedItem = (item as ComboBox).Items[0];
            }
            poruka.Text = "Rukovanje pilotima se radi kroz osobe";
            context.SaveChanges();
            izlistaj();
        }

        private void upravljanja_Click(object sender, RoutedEventArgs e)
        {
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            current = "upravljanja";
            if (upravljanja.Content.Equals("Dodaj/azuriraj"))
            {
                handlujAddUpravljanja();
                return;
            }
            resetujDugmice();
            izlistaj();
            prikaziUIUpravljanja();
            upravljanja.Content = "Dodaj/azuriraj";
            upravljanja.Foreground = new SolidColorBrush(Colors.Red);
        }
        private void prikaziUIUpravljanja()
        {
            TextBlock tbljmbg = new TextBlock();
            tbljmbg.Text = "UID:";
            tbljmbg.Margin = new Thickness() { Left = 610, Top = 175, Right = 0, Bottom = 0 };

            TextBox tbJMBG = new TextBox();
            tbJMBG.Name = "jmbg";
            tbJMBG.Text = "KEY";
            tbJMBG.Width = 100;
            tbJMBG.Height = 20;
            tbJMBG.Margin = new Thickness() { Left = 650, Top = 175, Right = 0, Bottom = 0 };

            TextBlock tbl = new TextBlock();
            tbl.Text = "Avion:";
            tbl.Margin = new Thickness() { Left = 610, Top = 210, Right = 0, Bottom = 0 };

            ComboBox cb = new ComboBox();
            var context = new BazeProjekatEntities();
            cb.Margin = new Thickness() { Left = 650, Top = 210, Right = 0, Bottom = 0 };
            cb.Width = 100;
            cb.Height = 20;
            cb.Name = "avion";
            cb.Items.Add("-");
            foreach (Avion o in context.Avion)
            {
                cb.Items.Add(o.AID);
            }

            TextBlock tbl1 = new TextBlock();
            tbl1.Text = "Pilot:";
            tbl1.Margin = new Thickness() { Left = 610, Top = 245, Right = 0, Bottom = 0 };

            ComboBox cb1 = new ComboBox();
            cb1.Margin = new Thickness() { Left = 650, Top = 245, Right = 0, Bottom = 0 };
            cb1.Width = 100;
            cb1.Height = 20;
            cb1.Items.Add("-");
            foreach (Pilot o in context.Pilot)
            {
                cb1.Items.Add(o.JMBG);
            }

            kanvas.Children.Add(tbl);
            kanvas.Children.Add(cb);
            kanvas.Children.Add(tbl1);
            kanvas.Children.Add(cb1);
            kanvas.Children.Add(tbJMBG);
            kanvas.Children.Add(tbljmbg);
            (cb as ComboBox).SelectedIndex = 0;
            (cb1 as ComboBox).SelectedIndex = 0;
        }  //BRISI ZA OSOBU I AVION
        private void handlujAddUpravljanja()
        {
            var context = new BazeProjekatEntities();
            Upravljati o = new Upravljati();
            foreach (object item in kanvas.Children)
            {
                if (item is ComboBox)
                {
                    if ((item as ComboBox).Name.Equals("avion"))
                    {
                        if ((item as ComboBox).SelectedIndex == 0)
                        {
                            poruka.Text = "Izaberi avion";
                            return;
                        }
                        o.AID = (item as ComboBox).SelectedItem.ToString();
                        o.Avion = context.Avion.ToList().Find(x => x.AID.Equals((item as ComboBox).SelectedItem));
                    }
                    else
                    {
                        if ((item as ComboBox).SelectedIndex == 0)
                        {
                            poruka.Text = "Izaberi pilota";
                            return;
                        }
                        o.JMBG = (item as ComboBox).SelectedItem.ToString();
                        o.Pilot = context.Pilot.ToList().Find(x => x.JMBG.Equals((item as ComboBox).SelectedItem));
                    }

                }
                if (!(item is TextBox))
                    continue;

                if ((item as TextBox).Name.Equals("jmbg"))
                {
                    if ((item as TextBox).Text.Equals(string.Empty) || (item as TextBox).Text.Equals("KEY"))
                    {
                        poruka.Text = "Unesite redovan kljuc!";
                        return;
                    }
                    object osoba = context.Upravljati.ToList().Find(x => x.UID.Equals((item as TextBox).Text));
                    if (osoba == null)
                    {
                        if(context.Upravljati.ToList().Find(x=>x.AID.Equals(o.AID) && x.JMBG.Equals(o.JMBG)) != null){
                            poruka.Text = "Bez duplikata!";
                            return;
                        }
                        o.UID = (item as TextBox).Text;
                        context.Upravljati.Add(o);
                    }
                    else
                    {
                        o.UID = (osoba as Upravljati).UID;
                        context.Upravljati.ToList().Find(x => x.Equals(osoba)).JMBG = o.JMBG;
                        context.Upravljati.ToList().Find(x => x.Equals(osoba)).Pilot = o.Pilot;
                        context.Upravljati.ToList().Find(x => x.Equals(osoba)).AID = o.AID;
                        context.Upravljati.ToList().Find(x => x.Equals(osoba)).Avion = o.Avion;
                    }
                }
            }
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            context.SaveChanges();
            izlistaj();
        }

        private void let_Click(object sender, RoutedEventArgs e)
        {
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            current = "let";
            if (let.Content.Equals("Dodaj/azuriraj"))
            {
                handlujAddLet();
                return;
            }
            resetujDugmice();
            izlistaj();
            prikaziUILet();
            let.Content = "Dodaj/azuriraj";
            let.Foreground = new SolidColorBrush(Colors.Red);
        }
        private void prikaziUILet()
        {
            TextBlock tbljmbg = new TextBlock();
            tbljmbg.Text = "LID:";
            tbljmbg.Margin = new Thickness() { Left = 610, Top = 175, Right = 0, Bottom = 0 };

            TextBox tbJMBG = new TextBox();
            tbJMBG.Name = "jmbg";
            tbJMBG.Text = "KEY";
            tbJMBG.Width = 100;
            tbJMBG.Height = 20;
            tbJMBG.Margin = new Thickness() { Left = 650, Top = 175, Right = 0, Bottom = 0 };


            TextBlock imprztbl = new TextBlock();
            imprztbl.Text = "Avion:";
            imprztbl.Margin = new Thickness() { Left = 610, Top = 280, Right = 0, Bottom = 0 };

            ComboBox cb0 = new ComboBox();
            var context = new BazeProjekatEntities();
            cb0.Margin = new Thickness() { Left = 650, Top = 280, Right = 0, Bottom = 0 };
            cb0.Width = 100;
            cb0.Height = 20;
            cb0.Name = "avion";
            cb0.Items.Add("-");
            foreach (Avion o in context.Avion)
            {
                cb0.Items.Add(o.AID);
            }

            TextBlock tbl = new TextBlock();
            tbl.Text = "Pista:";
            tbl.Margin = new Thickness() { Left = 610, Top = 245, Right = 0, Bottom = 0 };

            ComboBox cb = new ComboBox();
            cb.Margin = new Thickness() { Left = 650, Top = 245, Right = 0, Bottom = 0 };
            cb.Width = 100;
            cb.Height = 20;
            cb.Name = "pista";
            cb.Items.Add("-");
            foreach (Pista o in context.Pista)
            {
                cb.Items.Add(o.PID);
            }

            TextBlock tbl1 = new TextBlock();
            tbl1.Text = "Radnik:";
            tbl1.Margin = new Thickness() { Left = 610, Top = 210, Right = 0, Bottom = 0 };

            ComboBox cb1 = new ComboBox();
            cb1.Margin = new Thickness() { Left = 650, Top = 210, Right = 0, Bottom = 0 };
            cb1.Width = 100;
            cb1.Name = "radnik";
            cb1.Height = 20;
            cb1.Items.Add("-");
            foreach (Radnik o in context.Radnik)
            {
                cb1.Items.Add(o.JMBG);
            }

            kanvas.Children.Add(cb1);
            kanvas.Children.Add(cb0);
            kanvas.Children.Add(cb);
            kanvas.Children.Add(tbl);
            kanvas.Children.Add(tbl1);
            kanvas.Children.Add(tbJMBG);
            kanvas.Children.Add(tbljmbg);
            kanvas.Children.Add(imprztbl);
            (cb as ComboBox).SelectedIndex = 0;
            (cb1 as ComboBox).SelectedIndex = 0;
            (cb0 as ComboBox).SelectedIndex = 0;
        }
        private void handlujAddLet()
        {
            var context = new BazeProjekatEntities();
            Let o = new Let();
            foreach (object item in kanvas.Children)
            {
                if (item is ComboBox)
                {
                    if ((item as ComboBox).Name.Equals("avion"))
                    {
                        if ((item as ComboBox).SelectedIndex == 0)
                        {
                            poruka.Text = "Izaberi avion";
                            return;
                        }
                        o.AID_FK = (item as ComboBox).SelectedItem.ToString();
                        o.Avion = context.Avion.ToList().Find(x => x.AID.Equals((item as ComboBox).SelectedItem));
                    }
                    else if ((item as ComboBox).Name.Equals("pista"))
                    {
                        if ((item as ComboBox).SelectedIndex == 0)
                        {
                            poruka.Text = "Izaberi pistu";
                            return;
                        }
                        o.PID_FK = (item as ComboBox).SelectedItem.ToString();
                        o.Pista = context.Pista.ToList().Find(x => x.PID.Equals((item as ComboBox).SelectedItem));
                    }
                    else if ((item as ComboBox).Name.Equals("radnik"))
                    {
                        if ((item as ComboBox).SelectedIndex == 0)
                        {
                            continue;
                        }
                        o.JMBG_FK = (item as ComboBox).SelectedItem.ToString();
                        o.Radnik = context.Radnik.ToList().Find(x => x.JMBG.Equals((item as ComboBox).SelectedItem));
                    }

                }
                if (!(item is TextBox))
                    continue;

                if ((item as TextBox).Name.Equals("jmbg"))
                {
                    if ((item as TextBox).Text.Equals(string.Empty) || (item as TextBox).Text.Equals("KEY"))
                    {
                        poruka.Text = "Unesite redovan kljuc!";
                        return;
                    }
                    object osoba = context.Let.ToList().Find(x => x.LID.Equals((item as TextBox).Text));
                    if (osoba == null)
                    {
                        o.LID = (item as TextBox).Text;
                        context.Let.Add(o);
                    }
                    else
                    {
                        context.Let.Remove(osoba as Let);
                        context.SaveChanges();
                        o.LID = (item as TextBox).Text;
                        context.Let.Add(o);
                    }
                }
                
            }
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            try
            {
                context.SaveChanges();
            }
            catch(Exception e)
            {
                poruka.Text = e.InnerException.InnerException.Message;
            }
            izlistaj();
        }

        private void avion_Click(object sender, RoutedEventArgs e)
        {
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            current = "avion";
            if (avion.Content.Equals("Dodaj/azuriraj"))
            {
                handlujAddAvion();
                return;
            }
            resetujDugmice();
            izlistaj();
            prikaziUIAvion();
            avion.Content = "Dodaj/azuriraj";
            avion.Foreground = new SolidColorBrush(Colors.Red);
        }
        private void prikaziUIAvion()
        {
            TextBlock tbljmbg = new TextBlock();
            tbljmbg.Text = "AID:";
            tbljmbg.Margin = new Thickness() { Left = 610, Top = 175, Right = 0, Bottom = 0 };

            TextBox tbJMBG = new TextBox();
            tbJMBG.Name = "jmbg";
            tbJMBG.Text = "KEY";
            tbJMBG.Width = 100;
            tbJMBG.Height = 20;
            tbJMBG.Margin = new Thickness() { Left = 650, Top = 175, Right = 0, Bottom = 0 };

            TextBlock imprztbl = new TextBlock();
            imprztbl.Text = "Model:";
            imprztbl.Margin = new Thickness() { Left = 610, Top = 210, Right = 0, Bottom = 0 };

            TextBox IMPRZtb = new TextBox();
            IMPRZtb.Name = "imprz";
            IMPRZtb.Text = "model";
            IMPRZtb.Width = 100;
            IMPRZtb.Height = 20;
            IMPRZtb.Margin = new Thickness() { Left = 650, Top = 210, Right = 0, Bottom = 0 };

            kanvas.Children.Add(tbJMBG);
            kanvas.Children.Add(tbljmbg);
            kanvas.Children.Add(imprztbl);
            kanvas.Children.Add(IMPRZtb);
        }
        private void handlujAddAvion()
        {
            var context = new BazeProjekatEntities();
            Avion o = new Avion();
            bool update = false;
            foreach (object item in kanvas.Children)
            {
                if (!(item is TextBox))
                    continue;

                if ((item as TextBox).Name.Equals("jmbg"))
                {
                    if ((item as TextBox).Text.Equals(string.Empty) || (item as TextBox).Text.Equals("KEY"))
                    {
                        poruka.Text = "Unesite redovan kljuc!";
                        return;
                    }
                    object osoba = context.Avion.ToList().Find(x => x.AID.Equals((item as TextBox).Text));
                    if (osoba == null)
                    {
                        o.AID = (item as TextBox).Text;
                    }
                    else
                    {
                        o = osoba as Avion;
                        update = true;
                    }
                }
                if ((item as TextBox).Name.Equals("imprz"))
                {
                    if (update)
                    {
                        object osoba = context.Avion.ToList().Find(x => x.AID.Equals(o.AID));
                        if (o.Model.Equals((item as TextBox).Text))
                        {
                            poruka.Text = "Entitet je ostao isti";
                            return;
                        }

                        o.Model = (item as TextBox).Text;

                        context.Avion.Remove(osoba as Avion);
                        context.SaveChanges();

                        context.Avion.Add(o);
                        context.SaveChanges();
                    }
                    else
                    {
                        o.Model = (item as TextBox).Text;
                        context.Avion.Add(o);
                    }
                }
            }
            poruka.Text = "Oprez, unosom postojeceg kljuca birate da menjate postojeci entitet!!!";
            context.SaveChanges();
            izlistaj();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string jmbg = string.Empty;
            var context = new BazeProjekatEntities();
            foreach (var item in kanvas.Children)
            {
                if(item is TextBox)
                {
                    if((item as TextBox).Name.Equals("jmbg")) { 
                        if((item as TextBox).Text.Equals(string.Empty))
                        {
                            poruka.Text = "unesi postojeci kljuc!!!";
                            return;
                        }
                        jmbg = (item as TextBox).Text;
                    }
                }
            }
            switch (current)
            {
                case "osoba":
                    if(context.Osoba.ToList().Find(x=>x.JMBG.Equals(jmbg)) == null)
                    {
                        poruka.Text = "unesi postojeci kljuc!!!";
                        return;
                    }
                    context.Osoba.Remove(context.Osoba.ToList().Find(x => x.JMBG.Equals(jmbg)));
                    if(context.Putnik.ToList().Find(x=>x.JMBG.Equals(jmbg)) != null)
                    {
                        context.Putnik.Remove(context.Putnik.ToList().Find(x => x.JMBG.Equals(jmbg)));

                        if (context.Karta.ToList().Find(x => x.JMBG_FK.Equals(jmbg)) != null)
                        {
                            context.Karta.ToList().Find(x => x.JMBG_FK.Equals(jmbg)).Putnik = null;
                            context.Karta.ToList().Find(x => x.JMBG_FK.Equals(jmbg)).JMBG_FK = null;
                        }
                    }
                    else if (context.Radnik.ToList().Find(x => x.JMBG.Equals(jmbg)) != null)
                    {
                        context.Radnik.Remove(context.Radnik.ToList().Find(x => x.JMBG.Equals(jmbg)));
                        
                        if (context.Pilot.ToList().Find(x => x.JMBG.Equals(jmbg)) != null)
                        {
                            foreach (Upravljati u in context.Upravljati.ToList().FindAll(x=>x.JMBG.Equals(jmbg)))
                            {
                                context.Upravljati.Remove(u);
                            }
                            context.Pilot.Remove(context.Pilot.ToList().Find(x => x.JMBG.Equals(jmbg)));
                        }
                        if (context.Obavestenje.ToList().Find(x => x.JMBG_FK.Equals(jmbg)) != null)
                        {
                            context.Obavestenje.Remove(context.Obavestenje.ToList().Find(x => x.JMBG_FK.Equals(jmbg)));
                        }
                        bool doTheDoing = false;
                        foreach (Let le in context.Let)
                        {
                            if (le.JMBG_FK == null)
                                continue;
                            if (le.JMBG_FK.Equals(jmbg))
                            {
                                doTheDoing = true;
                                break;
                            }
                        }
                        if (doTheDoing)
                        {
                            foreach(Karta k in context.Karta.ToList().FindAll(x=>x.Let.Equals(context.Let.ToList().Find(y => y.JMBG_FK.Equals(jmbg)))))
                            {
                                context.Karta.Remove(k);
                            }
                            foreach(Let l in context.Let)
                            {
                                if (l.JMBG_FK == null)
                                    continue;
                                if (l.JMBG_FK.Equals(jmbg))
                                {
                                    l.Radnik = null;
                                    l.JMBG_FK = null;
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                    izlistaj();
                    break;
                case "let":
                    if (context.Let.ToList().Find(x => x.LID.Equals(jmbg)) == null)
                    {
                        poruka.Text = "unesi postojeci kljuc!!!";
                        return;
                    }
                    foreach (Karta k in context.Karta.ToList().FindAll(x => x.Let.LID.Equals(jmbg)))
                    {
                        context.Karta.Remove(k);
                    }
                    context.Let.Remove(context.Let.ToList().Find(x => x.LID.Equals(jmbg)));
                    context.SaveChanges();
                    izlistaj();
                    break;
                case "avion":
                    if (context.Avion.ToList().Find(x => x.AID.Equals(jmbg)) == null)
                    {
                        poruka.Text = "unesi postojeci kljuc!!!";
                        return;
                    }
                    foreach (Let k in context.Let.ToList().FindAll(x => x.AID_FK.Equals(jmbg)))
                    {
                        context.Let.Remove(k);
                    }
                    foreach (Upravljati u in context.Upravljati.ToList().FindAll(x => x.AID.Equals(jmbg)))
                    {
                        context.Upravljati.Remove(u);
                    }
                    context.Avion.Remove(context.Avion.ToList().Find(x => x.AID.Equals(jmbg)));
                    context.SaveChanges();
                    izlistaj();
                    break;
                case "pista":
                    if (context.Pista.ToList().Find(x => x.PID.Equals(jmbg)) == null)
                    {
                        poruka.Text = "unesi postojeci kljuc!!!";
                        return;
                    }
                    foreach (Let k in context.Let.ToList().FindAll(x => x.PID_FK.Equals(jmbg)))
                    {
                        context.Let.Remove(k);
                    }
                    context.Pista.Remove(context.Pista.ToList().Find(x => x.PID.Equals(jmbg)));
                    context.SaveChanges();
                    izlistaj();
                    break;
                case "obavestenje":
                    if (context.Obavestenje.ToList().Find(x => x.OID.Equals(jmbg)) == null)
                    {
                        poruka.Text = "unesi postojeci kljuc!!!";
                        return;
                    }
                    context.Obavestenje.Remove(context.Obavestenje.ToList().Find(x => x.OID.Equals(jmbg)));
                    context.SaveChanges();
                    izlistaj();
                    break;
                case "karta":
                    if (context.Karta.ToList().Find(x => x.KID.Equals(jmbg)) == null)
                    {
                        poruka.Text = "unesi postojeci kljuc!!!";
                        return;
                    }
                    context.Karta.Remove(context.Karta.ToList().Find(x => x.KID.Equals(jmbg)));
                    context.SaveChanges();
                    izlistaj();
                    break;
                case "upravljanja":
                    if (context.Upravljati.ToList().Find(x => x.UID.Equals(jmbg)) == null)
                    {
                        poruka.Text = "unesi postojeci kljuc!!!";
                        return;
                    }
                    context.Upravljati.Remove(context.Upravljati.ToList().Find(x => x.UID.Equals(jmbg)));
                    context.SaveChanges();
                    izlistaj();
                    break;
            }
        }
        private void resetujDugmice()
        {
            foreach(var item in grid.Children)
            {
                
                if(item is Button)
                {
                    if ((item as Button).Content.Equals("Brisi"))
                        continue;
                    (item as Button).Foreground = new SolidColorBrush(Colors.Navy);
                    switch ((item as Button).Name)
                    {
                        case "osoba":
                            (item as Button).Content = "Osoba";
                            break;
                        case "putnik":
                            (item as Button).Content = "Putnik";
                            break;
                        case "radnik":
                            (item as Button).Content = "Radnik";
                            break;
                        case "pilot":
                            (item as Button).Content = "Pilot";
                            break;
                        case "let":
                            (item as Button).Content = "Let";
                            break;
                        case "avion":
                            (item as Button).Content = "Avion";
                            break;
                        case "pista":
                            (item as Button).Content = "Pista";
                            break;
                        case "obavestenje":
                            (item as Button).Content = "Obavestenje";
                            break;
                        case "karta":
                            (item as Button).Content = "Karta";
                            break;
                        case "upravljanja":
                            (item as Button).Content = "Upravljanja";
                            break;
                    }
                }
            }
            kanvas.Children.Clear();
            tabla.Children.Clear();
        }
        private void izlistaj()
        {
            var context = new BazeProjekatEntities();
            switch (current)
            {
                case "osoba":
                    tabla.Children.Clear();
                    TextBlock t = new TextBlock();
                    t.Background = new SolidColorBrush(Colors.White);
                    t.Text ="JMBG\t\tIme_Prezime";
                    t.Background = new SolidColorBrush(Colors.Navy); t.Foreground = new SolidColorBrush(Colors.LightSteelBlue);
                    tabla.Children.Add(t);
                    foreach (Osoba o in context.Osoba)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Background = new SolidColorBrush(Colors.White);
                        tb.Text = o.JMBG + "\t\t" + o.ImPrz;
                        tabla.Children.Add(tb);
                    }
                    break;
                case "putnik":
                    tabla.Children.Clear();
                    TextBlock t1 = new TextBlock();
                    t1.Background = new SolidColorBrush(Colors.White);
                    t1.Text = "JMBG\t\tIme_Prezime";
                    t1.Background = new SolidColorBrush(Colors.Navy); t1.Foreground = new SolidColorBrush(Colors.LightSteelBlue);
                    tabla.Children.Add(t1);
                    foreach (Putnik o in context.Putnik)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Background = new SolidColorBrush(Colors.White);
                        tb.Text = o.JMBG + "\t\t" + o.ImPrz;
                        tabla.Children.Add(tb);
                    }
                    break;
                case "radnik":
                    tabla.Children.Clear();
                    tabla.Children.Clear();
                    TextBlock t2 = new TextBlock();
                    t2.Background = new SolidColorBrush(Colors.White);
                    t2.Text = "JMBG\t\tIme_Prezime";
                    t2.Background = new SolidColorBrush(Colors.Navy); t2.Foreground = new SolidColorBrush(Colors.LightSteelBlue);
                    tabla.Children.Add(t2);
                    foreach (Radnik o in context.Radnik)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Background = new SolidColorBrush(Colors.White);
                        tb.Text = o.JMBG + "\t\t" + o.ImPrz;
                        tabla.Children.Add(tb);
                    }
                    break;
                case "pilot":
                    tabla.Children.Clear();
                    TextBlock t3 = new TextBlock();
                    t3.Background = new SolidColorBrush(Colors.White);
                    t3.Text = "JMBG\t\tIme_Prezime";
                    t3.Background = new SolidColorBrush(Colors.Navy); t3.Foreground = new SolidColorBrush(Colors.LightSteelBlue);
                    tabla.Children.Add(t3);
                    foreach (Pilot o in context.Pilot)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Background = new SolidColorBrush(Colors.White);
                        tb.Text = o.JMBG + "\t\t" + o.ImPrz;
                        tabla.Children.Add(tb);
                    }
                    break;
                case "let":
                    tabla.Children.Clear();
                    TextBlock t4 = new TextBlock();
                    t4.Background = new SolidColorBrush(Colors.White);
                    t4.Text = "LID\t\tJMBG\tPistaID\tAvionID";
                    t4.Background = new SolidColorBrush(Colors.Navy); t4.Foreground = new SolidColorBrush(Colors.LightSteelBlue);
                    tabla.Children.Add(t4);
                    foreach (Let o in context.Let)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Background = new SolidColorBrush(Colors.White);
                        tb.Text = o.LID + "\t\t" + o.JMBG_FK + "\t" + o.PID_FK + "\t" + o.AID_FK;
                        tabla.Children.Add(tb);
                    }
                    break;
                case "avion":
                    tabla.Children.Clear();
                    TextBlock t5 = new TextBlock();
                    t5.Background = new SolidColorBrush(Colors.White);
                    t5.Text = "AvionID\t\tModel";
                    t5.Background = new SolidColorBrush(Colors.Navy); t5.Foreground = new SolidColorBrush(Colors.LightSteelBlue);
                    tabla.Children.Add(t5);
                    foreach (Avion o in context.Avion)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Background = new SolidColorBrush(Colors.White);
                        tb.Text = o.AID + "\t\t" + o.Model;
                        tabla.Children.Add(tb);
                    }
                    break;
                case "pista":
                    tabla.Children.Clear();
                    TextBlock t6 = new TextBlock();
                    t6.Background = new SolidColorBrush(Colors.White);
                    t6.Text = "PistaID\t\tKula";
                    t6.Background = new SolidColorBrush(Colors.Navy); t6.Foreground = new SolidColorBrush(Colors.LightSteelBlue);
                    tabla.Children.Add(t6);
                    foreach (Pista o in context.Pista)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Background = new SolidColorBrush(Colors.White);
                        tb.Text = o.PID + "\t\t" + o.Kula;
                        tabla.Children.Add(tb);
                    }
                    break;
                case "obavestenje":
                    tabla.Children.Clear();
                    TextBlock t7 = new TextBlock();
                    t7.Background = new SolidColorBrush(Colors.White);
                    t7.Text = "ObavestenjeID\t\tJMBG\tPoruka";
                    t7.Background = new SolidColorBrush(Colors.Navy); t7.Foreground = new SolidColorBrush(Colors.LightSteelBlue);
                    tabla.Children.Add(t7);
                    foreach (Obavestenje o in context.Obavestenje)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Background = new SolidColorBrush(Colors.White);
                        tb.Text = o.OID + "\t\t" + o.JMBG_FK + "\t" + o.Poruka;
                        tabla.Children.Add(tb);
                    }
                    break;
                case "karta":
                    tabla.Children.Clear();
                    TextBlock t8 = new TextBlock();
                    t8.Background = new SolidColorBrush(Colors.White);
                    t8.Text = "KartaID\t\tKlasa\tJMBG\tLetID";
                    t8.Background = new SolidColorBrush(Colors.Navy); t8.Foreground = new SolidColorBrush(Colors.LightSteelBlue);
                    tabla.Children.Add(t8);
                    foreach (Karta o in context.Karta)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Background = new SolidColorBrush(Colors.White);
                        tb.Text = o.KID + "\t\t" + o.Klasa + "\t" + o.JMBG_FK + "\t" + o.LID_FK;
                        tabla.Children.Add(tb);
                    }
                    break;
                case "upravljanja":
                    tabla.Children.Clear();
                    TextBlock t9 = new TextBlock();
                    t9.Background = new SolidColorBrush(Colors.White);
                    t9.Text = "UpravljanjeID\t\tAvionID\tJMBG\tJMBG";
                    t9.Background = new SolidColorBrush(Colors.Navy); t9.Foreground = new SolidColorBrush(Colors.LightSteelBlue);
                    tabla.Children.Add(t9);
                    foreach (Upravljati o in context.Upravljati)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Background = new SolidColorBrush(Colors.White);
                        tb.Text = o.UID + "\t\t" + o.AID + "\t" + o.JMBG;
                        tabla.Children.Add(tb);
                    }
                    break;
            }
        }

        private void Prikazi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var conString = ConfigurationManager.ConnectionStrings["BazeProjekatEntities"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlCommand com = new SqlCommand("spPilotAvion", con))
                    {
                        com.CommandType = System.Data.CommandType.StoredProcedure;

                        try
                        {
                            
                        }
                        catch
                        {
                            MessageBox.Show("Neka greska se desila", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                }
            }
            catch { }
        }
    }
}
