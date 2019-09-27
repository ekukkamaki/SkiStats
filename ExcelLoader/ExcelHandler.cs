using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entities.Models;
using Syncfusion.XlsIO;

namespace excelLoader
{
  public static class ExcelHandler
  {
    private static readonly Dictionary<string, int> _monthDic = new Dictionary<string, int>() { { "Tammikuu", 1 }, { "Helmikuu", 2 }, { "Maaliskuu", 3 }, { "Huhtikuu", 4 }, { "Toukokuu", 5 }, { "Kesäkuu", 6 }, { "Heinäkuu", 7 }, { "Elokuu", 8 }, { "Syyskuu", 9 }, { "Lokakuu", 10 }, { "Marraskuu", 11 }, { "Joulukuu", 12 } };
    private static readonly string[] _nextYearSkiMonths = new string[] { "Tammikuu", "Helmikuu", "Maaliskuu", "Huhtikuu" };

    public static void SeedDbForExcelData(Entities.RepositoryContext context, string fileName, Entities.Models.Person person)
    {
      var roundRepository = new Repository.RoundRepository(context);
      var personRepository = new Repository.PersonRepository(context);
      var locationRepository = new Repository.LocationRepository(context);

      // do nothing if current person exists 
      if (personRepository.FindByCondition(s => s.Name == person.Name).ToList().Count > 0)
        return;

      var allSavedLocations = locationRepository.FindAll().ToList();

      using (ExcelEngine excelEngine = new ExcelEngine())
      {
        IApplication application = excelEngine.Excel;

        application.DefaultVersion = ExcelVersion.Excel2016;

        personRepository.Create(person);

        using (FileStream fs = File.OpenRead(Path.Combine(@"..\\ExcelLoader\\data\\", fileName)))
        {
          IWorkbook wb = application.Workbooks.Open(fs);
          IWorksheet ws = wb.Worksheets[0];
          var usedRowsLength = ws.UsedRange.Rows.Length;


          for (int i = 0; i < usedRowsLength; i++)
          {
            if (i == 0)
              continue;

            if (string.IsNullOrEmpty((string)ws.Range["A" + (i + 1)].Value2))
              break;

            DateTime actualDate = getDate((string)ws.Range["A" + (i + 1)].Value2,
            (string)ws.Range["C" + (i + 1)].Value2,
            (string)ws.Range["E" + (i + 1)].Value2.ToString(), (i + 1));
            Console.WriteLine("date is: " + actualDate.ToString());

            double routeLenght = (double)ws.Range["G" + (i + 1)].Value2;
            string locationName = (string)ws.Range["H" + (i + 1)].Value2;

            var location = allSavedLocations.SingleOrDefault(l => { return l.Name == locationName; });
            //do not add location if not known
            if (location == null && !string.IsNullOrEmpty(locationName.TrimStart().TrimEnd()))
            {
              location = new Location
              {
                Name = locationName
              };
              locationRepository.Create(location);
              allSavedLocations.Add(location);
            }

            var round = new Entities.Models.Round
            {
              TotalKilometers = routeLenght,
              SkiStyle = 1,
              ActionTime = actualDate,
              Person = person,
              Location = location
            };
            roundRepository.CreateRound(round);
          }
        }
      }
    }
    private static DateTime getDate(string YearInExcelFormat, string monthFromExcel, string dayFromExcel, int row)
    {
      int year;
      int monthOfYear;
      int dayOfMonth;
      Console.WriteLine("dayFromExcel" + dayFromExcel);

      if (string.IsNullOrEmpty(YearInExcelFormat) || YearInExcelFormat.Length < 4 || !int.TryParse(YearInExcelFormat.Substring(0, 4), out year))
        throw new FormatException("Cannot find year in correct format. Year: " + YearInExcelFormat + ", row: " + row);
      if (string.IsNullOrEmpty(monthFromExcel) || !_monthDic.TryGetValue(monthFromExcel, out monthOfYear) || monthOfYear > 12 || monthOfYear < 1)
        throw new FormatException("Cannot find month in correct format. Month: " + monthFromExcel + ", row: " + row);
      if (string.IsNullOrEmpty(dayFromExcel) || !int.TryParse(dayFromExcel, out dayOfMonth))
        throw new FormatException("Cannot find day in correct format. Day: " + dayFromExcel + ", row: " + row);

      year = _nextYearSkiMonths.Equals(monthFromExcel) ? year + 1 : year;

      Console.WriteLine("Converting proper date. year:" + year + ", month:" + monthOfYear + ", day:" + dayOfMonth + ", rowNumber:" + row);
      return new DateTime(year, monthOfYear, dayOfMonth);
    }
  }
}