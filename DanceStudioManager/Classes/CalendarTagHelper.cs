using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DanceStudioManager.Classes
{
        [HtmlTargetElement("calendar", TagStructure = TagStructure.NormalOrSelfClosing)]
        public class CalendarTagHelper : TagHelper
        {
            public bool ShowCurrentWeek { get; set; } = false;
            public int Month { get; set; }
            public int Year { get; set; }
            public CalendarSearchVM Events { get; set; }


            public override void Process(TagHelperContext context, TagHelperOutput output)
            {
                output.TagName = "section";
                output.Attributes.Add("class", "calendar");
                output.Content.SetHtmlContent(GetHtml());
                output.TagMode = TagMode.StartTagAndEndTag;
            }

            private string GetHtml()
            {
                var monthStart = new DateTime(Year, Month, 1);
                List<CalendarData> leaves = Events.CalendarData;

                var html = new XDocument(
                    new XElement("div",
                        new XAttribute("class", "container-fluid h"),
                        new XElement("header",
                            new XElement("div",
                                new XAttribute("class", "row"),

                                    new XElement("div",
                                        new XAttribute("class", "pull-left"),
                                    new XElement("button",
                                        new XAttribute("id", "prevBtn"),
                                       new XAttribute("type", "button"),
                                        new XAttribute("class", "navbar-calendar btn  previous"),
                                        "<<")
                                  ),

                                new XElement("div",
                                    new XAttribute("class", "col"),
                                new XElement("h5",
                                    new XAttribute("class", "col display-4 mb-5 text-center capitalize"),
                                    monthStart.ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("en-EN"))
                                    )
                                ),

                                    new XElement("div",
                                        new XAttribute("class", "pull-right"),
                                        new XElement("button",
                                            new XAttribute("id", "nextBtn"),
                                            new XAttribute("type", "button"),
                                            new XAttribute("class", "navbar-calendar btn next"),
                                            ">>")

                                )
                            ),
                            new XElement("div",
                                new XAttribute("class", "row d-none d-lg-flex p-1 week text-black"),
                                (new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" }).Select(d =>
                                       new XElement("h5",
                                           new XAttribute("class", "col-lg p-1 text-center"),
                                           d.ToString()
                                       )
                                )
                            )

                        ),
                        new XElement("div",
                            new XAttribute("class", "row border border-right-0 border-bottom-0"),
                            GetDatesHtml()
                        )
                    )
                );

                return html.ToString();

                IEnumerable<XElement> GetDatesHtml()
                {

                    DateTime startPeriod = ShowCurrentWeek ? DateTime.Now.Date : monthStart;
                    var startDate = startPeriod.AddDays(-((int)startPeriod.DayOfWeek == 0 ? 6 : (int)startPeriod.DayOfWeek - 1));
                    int endOfRange = ShowCurrentWeek ? 6 : 42;
                    var dates = Enumerable.Range(0, endOfRange).Select(i => startDate.AddDays(i));

                    foreach (var d in dates)
                    {
                        if (d.DayOfWeek == DayOfWeek.Monday && d != startDate)
                        {
                            yield return new XElement("div",
                                new XAttribute("class", "w-100"),
                                String.Empty
                            );
                        }

                        var mutedClasses = "d-none d-lg-inline-block bg-light text-muted";
                        var holidayClasses = "holidayClasses";
                        var currentDay = "currentDay";

                    yield return new XElement("div",
                        new XAttribute("class", $"day col-lg p-2 border border-left-0 border-top-0 text-truncate {(d.Month != monthStart.Month ? mutedClasses : null)} {(Events.Days[d.DayOfYear-1].WorkDay ? null : holidayClasses)} {(Events.Days[d.DayOfYear-1].Day == DateTime.Now.Date ? currentDay : null)}"),
                            new XElement("h5",
                                new XAttribute("class", "row align-items-center"),
                                new XElement("span",
                                    new XAttribute("class", "date col-1"),
                                    d.Day
                                ),
                                new XElement("small",
                                    new XAttribute("class", "col d-lg-none text-center text-muted"),
                                    d.DayOfWeek.ToString()
                                ),
                                new XElement("span",
                                    new XAttribute("class", "col-1"),
                                    String.Empty
                                )
                            ),
                            GetEventHtml(d)
                        );
                    }
                }

                IEnumerable<XElement> GetEventHtml(DateTime d)
                {
                    CalendarEventGenerator generator = new CalendarEventGenerator(Events, d);
                    List<XElement> elements = generator.GetEventElements();
                    if (elements.Count == 0)
                    {
                        elements.Add(new XElement("p",
                        new XAttribute("class", "d-lg-none"),
                        "No events"
                    ));
                    }
                    return elements;
                }
            }

            private bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
            {
                var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
                var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
                var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

                return d1 == d2;
            }
        }
    }
