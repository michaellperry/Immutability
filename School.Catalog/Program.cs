using Jinaga;
using School.Model;

Console.WriteLine("Receiving courses");

var j = JinagaClient.Create(options =>
{
    options.HttpEndpoint = new Uri("http://localhost:8080/jinaga/");
});

var coursesAtInstitution = Given<Institution>.Match((institution, facts) =>
  from course in facts.OfType<Course>()
  where course.institution == institution
  select new
  {
    course,
    title = facts.Observable(
        facts.OfType<CourseTitle>()
            .Where(title => title.course == course &&
                !facts.Any<CourseTitle>(next => next.prior.Contains(title))
            )
            .Select(title => title.value)
    ),
    credits = facts.Observable(
        facts.OfType<CourseCredits>()
            .Where(credits => credits.course == course &&
                !facts.Any<CourseCredits>(next => next.prior.Contains(credits))
            )
            .Select(credits => credits.value)
    )
  }
);

var universityOfTulsa = await j.Fact(new Institution("03185"));

var observer = j.Subscribe(coursesAtInstitution, universityOfTulsa, projection =>
{
    Console.WriteLine($"INSERT Course {projection.course.code}");
    projection.title.OnAdded(title =>
    {
        Console.WriteLine($"UPDATE Course {projection.course.code} SET Title = \"{title}\"");
    });
    projection.credits.OnAdded(credits =>
    {
        Console.WriteLine($"UPDATE Course {projection.course.code} SET Credits = {credits}");
    });
});

Console.WriteLine("Press any key to stop");
Console.ReadKey();

observer.Stop();