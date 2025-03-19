using System;
using System.Collections.Generic;

namespace ZarzadzanieKontenerami
{
    class KontenerCiekly
    {
        private static int licznik = 1;
        public string NumerSeryjny { get; }
        public double Pojemnosc { get; }
        public double AktualnyLadunek { get; private set; }
        public bool CzyNiebezpieczny { get; }

        public KontenerCiekly(double pojemnosc, bool czyNiebezpieczny)
        {
            NumerSeryjny = "KON-L-" + licznik++;
            Pojemnosc = pojemnosc;
            CzyNiebezpieczny = czyNiebezpieczny;
            AktualnyLadunek = 0;
        }

        public void Zaladuj(double ilosc)
        {
            double limit = CzyNiebezpieczny ? Pojemnosc * 0.5 : Pojemnosc * 0.9;
            if (AktualnyLadunek + ilosc > limit)
                throw new Exception($"Kontener {NumerSeryjny}: próba załadowania {ilosc} kg przekracza {limit} kg.");
            AktualnyLadunek += ilosc;
            Console.WriteLine($"Załadowano {ilosc} kg do {NumerSeryjny}. Aktualny: {AktualnyLadunek} kg.");
        }

        public void Rozladuj()
        {
            Console.WriteLine($"Rozładowano {AktualnyLadunek} kg z {NumerSeryjny} (ciekły).");
            AktualnyLadunek = 0;
        }
    }

    class KontenerGazowy
    {
        private static int licznik = 1;
        public string NumerSeryjny { get; }
        public double Pojemnosc { get; }
        public double AktualnyLadunek { get; private set; }
        public double Cisnienie { get; }

        public KontenerGazowy(double pojemnosc, double cisnienie)
        {
            NumerSeryjny = "KON-G-" + licznik++;
            Pojemnosc = pojemnosc;
            Cisnienie = cisnienie;
            AktualnyLadunek = 0;
        }

        public void Zaladuj(double ilosc)
        {
            double limit = Pojemnosc * 0.5;
            if (AktualnyLadunek + ilosc > limit)
                throw new Exception($"Kontener {NumerSeryjny}: próba załadowania {ilosc} kg przekracza {limit} kg.");
            AktualnyLadunek += ilosc;
            Console.WriteLine($"Załadowano {ilosc} kg do {NumerSeryjny}. Aktualny: {AktualnyLadunek} kg.");
        }

        public void Rozladuj()
        {
            double rozladowano = AktualnyLadunek * 0.95;
            AktualnyLadunek *= 0.05;
            Console.WriteLine($"Rozładowano {rozladowano} kg z {NumerSeryjny} (gazowy). Pozostało {AktualnyLadunek} kg.");
        }
    }

    class KontenerChlodniczy
    {
        private static int licznik = 1;
        public string NumerSeryjny { get; }
        public double Pojemnosc { get; }
        public double AktualnyLadunek { get; private set; }
        public double Temperatura { get; }
        public double TemperaturaWymagana { get; }

        public KontenerChlodniczy(double pojemnosc, double temperaturaWymagana, double temperatura)
        {
            NumerSeryjny = "KON-C-" + licznik++;
            Pojemnosc = pojemnosc;
            TemperaturaWymagana = temperaturaWymagana;
            Temperatura = temperatura;
            AktualnyLadunek = 0;
        }

        public void Zaladuj(double ilosc)
        {
            if (Temperatura < TemperaturaWymagana)
                throw new Exception($"Kontener {NumerSeryjny}: temperatura {Temperatura}°C jest za niska.");
            double limit = Pojemnosc * 0.9;
            if (AktualnyLadunek + ilosc > limit)
                throw new Exception($"Kontener {NumerSeryjny}: próba załadowania {ilosc} kg przekracza {limit} kg.");
            AktualnyLadunek += ilosc;
            Console.WriteLine($"Załadowano {ilosc} kg do {NumerSeryjny}. Aktualny: {AktualnyLadunek} kg.");
        }

        public void Rozladuj()
        {
            Console.WriteLine($"Rozładowano {AktualnyLadunek} kg z {NumerSeryjny} (chłodniczy).");
            AktualnyLadunek = 0;
        }
    }

    class Statek
    {
        public string Nazwa { get; }
        public int MaksKontenerow { get; }
        private List<object> kontenery = new List<object>();

        public Statek(string nazwa, int maksKontenerow)
        {
            Nazwa = nazwa;
            MaksKontenerow = maksKontenerow;
        }

        public void DodajKontener(object kontener)
        {
            if (kontenery.Count >= MaksKontenerow)
                throw new Exception($"Statek {Nazwa} jest pełny.");
            if (kontener is KontenerCiekly c)
            {
                double limit = c.CzyNiebezpieczny ? c.Pojemnosc * 0.5 : c.Pojemnosc * 0.9;
                if (c.AktualnyLadunek > limit)
                    throw new Exception($"Kontener {c.NumerSeryjny} jest przeładowany.");
            }
            else if (kontener is KontenerGazowy g)
            {
                double limit = g.Pojemnosc * 0.5;
                if (g.AktualnyLadunek > limit)
                    throw new Exception($"Kontener {g.NumerSeryjny} jest przeładowany.");
            }
            else if (kontener is KontenerChlodniczy ch)
            {
                double limit = ch.Pojemnosc * 0.9;
                if (ch.AktualnyLadunek > limit)
                    throw new Exception($"Kontener {ch.NumerSeryjny} jest przeładowany.");
            }
            kontenery.Add(kontener);
            Console.WriteLine($"Dodano kontener do statku {Nazwa}.");
        }

        public void WyswietlInformacje()
        {
            Console.WriteLine($"\nStatek {Nazwa} przewozi {kontenery.Count}/{MaksKontenerow} kontenerów.");
            foreach (var k in kontenery)
            {
                if (k is KontenerCiekly c)
                    Console.WriteLine($" - {c.NumerSeryjny}: {c.AktualnyLadunek}/{c.Pojemnosc} kg (ciekły)");
                else if (k is KontenerGazowy g)
                    Console.WriteLine($" - {g.NumerSeryjny}: {g.AktualnyLadunek}/{g.Pojemnosc} kg (gazowy)");
                else if (k is KontenerChlodniczy ch)
                    Console.WriteLine($" - {ch.NumerSeryjny}: {ch.AktualnyLadunek}/{ch.Pojemnosc} kg (chłodniczy)");
            }
        }

        public void RozladujWszystko()
        {
            foreach (var k in kontenery)
            {
                if (k is KontenerCiekly c) c.Rozladuj();
                if (k is KontenerGazowy g) g.Rozladuj();
                if (k is KontenerChlodniczy ch) ch.Rozladuj();
            }
            Console.WriteLine($"Rozładowano wszystkie kontenery na statku {Nazwa}.");
        }
    }

    class Program
    {
        static void Main()
        {
            Statek st1 = new Statek("Hercules", 3);
            Statek st2 = new Statek("Poseidon", 3);

            KontenerCiekly k1 = new KontenerCiekly(1000, false);
            KontenerGazowy k2 = new KontenerGazowy(2000, 5);
            KontenerChlodniczy k3 = new KontenerChlodniczy(5000, 2, 5);
            KontenerCiekly k4 = new KontenerCiekly(800, false);

            try
            {
                k1.Zaladuj(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                k2.Zaladuj(900);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                k3.Zaladuj(4000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                st1.DodajKontener(k1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                st1.DodajKontener(k2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                st1.DodajKontener(k3);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                k4.Zaladuj(600);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                st1.DodajKontener(k4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                st2.DodajKontener(k4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            st1.WyswietlInformacje();
            st2.WyswietlInformacje();

            try
            {
                st1.RozladujWszystko();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            st1.WyswietlInformacje();
            st2.WyswietlInformacje();

            Console.WriteLine("Koniec programu.");
        }
    }
}
