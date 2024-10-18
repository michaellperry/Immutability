namespace School.Model;

using Jinaga;

[FactType("School.Institution")]
public record Institution(string identifier) {}

[FactType("School.Course")]
public record Course(Institution institution, string code) {}

[FactType("School.Course.Title")]
public record CourseTitle(Course course, string title, CourseTitle[] prior) {}

[FactType("School.Course.Credits")]
public record CourseCredits(Course course, int credits, CourseCredits[] prior) {}