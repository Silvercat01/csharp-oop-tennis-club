using Xunit;
using nagybeadando_teniszpalya;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TennisClubTests
{
    public class TennisClubTests
    {
        private Klub CreateTestClub()
        {
            var tagok = new List<Tag> { new Tag("Test Member", Tag.Kedvezmeny.Altalanos, new AltalanosPricingStrategy()) };
            var palyak = new List<Palya>
            {
                new Palya(1, Palya.Boritas.Salak, true),
                new Palya(2, Palya.Boritas.Fuves, false)
            };
            var foglalasok = new List<Foglalas>();

            var klub = new Klub(tagok, palyak, foglalasok, null);
            klub.foglalaskezelo = new Foglalaskezelo(klub);

            return klub;
        }

        //Tag létrehozása és adatok ellenõrzése
        [Fact]
        public void TagCreation_WithValidData_Success()
        {
            var strategy = new DiakPricingStrategy();
            var tag = new Tag("Test Member", Tag.Kedvezmeny.Altalanos, strategy);
            Assert.Equal("Test Member", tag.nev);
        }

        //Kedvezményes díjszámítás(Diák, Általános stb.)
        [Fact]
        public void CalculateUsageFee_CorrectForDiak()
        {
            var klub = CreateTestClub();
            var tag = new Tag("Diák Tag", Tag.Kedvezmeny.Diak, new DiakPricingStrategy());
            klub.beleptet(tag);

            var court = klub.palyak.First();
            var datum = DateTime.Today;

            klub.foglalaskezelo.Foglal(tag, court, datum, 10);
            klub.foglalasok.First().allapot = Foglalas.Allapot.Teljesitett;

            var fee = klub.foglalaskezelo.tagHasznalatiDij(tag, datum);
            var expected = (int)(court.getOradij() * 0.8);

            Assert.Equal(expected, fee);
        }

        //Pályaóradíj számítása
        [Fact]
        public void CourtPriceCalculation_FedettSalak_CorrectPrice()
        {
            var court = new Palya(1, Palya.Boritas.Salak, true);
            Assert.Equal(3600, court.getOradij()); // 3000 * 1.2
        }

        //Sikeres foglalás
        [Fact]
        public void Reservation_AvailableCourt_Success()
        {
            var klub = CreateTestClub();
            var tag = klub.tagok.First();
            var court = klub.palyak.First();

            var result = klub.foglalaskezelo.Foglal(tag, court, DateTime.Today, 10);

            Assert.True(result);
            Assert.Single(klub.foglalasok);
        }

        //Ütközõ foglalás elutasítása
        [Fact]
        public void Reservation_SameTimeSameCourt_Fails()
        {
            var klub = CreateTestClub();
            var tag = klub.tagok.First();
            var court = klub.palyak.First();
            var datum = DateTime.Today;
            var ora = 10;

            var first = klub.foglalaskezelo.Foglal(tag, court, datum, ora);
            var second = klub.foglalaskezelo.Foglal(tag, court, datum, ora);

            Assert.True(first);
            Assert.False(second);
        }

        //Foglalás lemondása
        [Fact]
        public void CancelReservation_Success()
        {
            var klub = CreateTestClub();
            var tag = klub.tagok.First();
            var court = klub.palyak.First();
            var datum = DateTime.Today;

            klub.foglalaskezelo.Foglal(tag, court, datum, 9);
            var foglalas = klub.foglalasok.First();

            var result = klub.foglalaskezelo.Lemond(foglalas);

            Assert.True(result);
            Assert.Empty(klub.foglalasok);
        }

        //Napi foglalások listázása
        [Fact]
        public void TagNapiFoglalasai_ReturnsCorrectReservations()
        {
            var klub = CreateTestClub();
            var tag = klub.tagok.First();
            var court = klub.palyak.First();
            var datum = DateTime.Today;

            klub.foglalaskezelo.Foglal(tag, court, datum, 8);
            klub.foglalaskezelo.Foglal(tag, court, datum, 9);

            var foglalasok = klub.foglalaskezelo.TagNapiFoglalasai(tag, datum);

            Assert.Equal(2, foglalasok.Count);
        }

    }
}