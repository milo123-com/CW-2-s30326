// Program.cs
using System;
using System.Collections.Generic;

namespace ZarzadzanieKontenerami
{
    // Prosta klasa dla kontenerów ciekłych
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
            {
                Console.WriteLine($"Błąd: Nie można załadować {ilosc} kg do {NumerSeryjny}. Przekroczono limit.");
                return;
            }
            AktualnyLadunek += ilosc;
            Console.WriteLine($"Załadowano {ilosc} kg do {NumerSeryjny}. Aktualny ładunek: {AktualnyLadunek} kg.");
        }

        public void Rozladuj()
        {
            Console.WriteLine($"Rozładowano {AktualnyLadunek} kg z {NumerSeryjny}.");
            AktualnyLadunek = 0;
        }
    }

    // Prosta klasa dla kontenerów gazowych
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
            {
                Console.WriteLine($"Błąd: Nie można załadować {ilosc} kg do {NumerSeryjny}. Przekroczono limit.");
                return;
            }
            AktualnyLadunek += ilosc;
            Console.WriteLine($"Załadowano {ilosc} kg do {NumerSeryjny}. Aktualny ładunek: {AktualnyLadunek} kg.");
        }

        public void Rozladuj()
        {
            double rozladowano = AktualnyLadunek * 0.95;
            AktualnyLadunek *= 0.05;
            Console.WriteLine($"Rozładowano {rozladowano} kg z {NumerSeryjny}. Pozostało: {AktualnyLadunek} kg.");
        }
    }

    // Prosta klasa dla kontenerów chłodniczych
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
            {
                Console.WriteLine($"Błąd: Temperatura zbyt niska dla {NumerSeryjny}.");
                return;
            }
            double limit = Pojemnosc * 0.9;
            if (AktualnyLadunek + ilosc > limit)
            {
                Console.WriteLine($"Błąd: Nie można załadować {ilosc} kg do {NumerSeryjny}. Przekroczono limit.");
                return;
            }
            AktualnyLadunek += ilosc;
            Console.WriteLine($"Załadowano {ilosc} kg do {NumerSeryjny}. Aktualny ładunek: {AktualnyLadunek} kg.");
        }

        public void Rozladuj()
        {
            Console.WriteLine($"Rozładowano {AktualnyLadunek} kg z {NumerSeryjny}.");
            AktualnyLadunek = 0;
        }
    }

    // Prosta klasa statku
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
            {
                Console.WriteLine("Błąd: Statek jest pełny.");
                return;
            }
            kontenery.Add(kontener);
            Console.WriteLine($"Dodano kontener do statku {Nazwa}.");
        }

        public void WyswietlInformacje()
        {
            Console.WriteLine($"Statek {Nazwa} przewozi {kontenery.Count}/{MaksKontenerow} kontenerów.");
        }
    }

    class Program
    {
        static void Main()
        {
            Statek statek = new Statek("Titanic", 5);

            KontenerCiekly kontenerCiekly = new KontenerCiekly(10000, false);
            kontenerCiekly.Zaladuj(8000);
            statek.DodajKontener(kontenerCiekly);

            KontenerGazowy kontenerGazowy = new KontenerGazowy(2000, 5);
            kontenerGazowy.Zaladuj(1000);
            statek.DodajKontener(kontenerGazowy);

            KontenerChlodniczy kontenerChlodniczy = new KontenerChlodniczy(8000, 4, 5);
            kontenerChlodniczy.Zaladuj(7000);
            statek.DodajKontener(kontenerChlodniczy);

            statek.WyswietlInformacje();
        }
    }
}
