var projectName = "Orc.FileAssociation";
var projectsToPackage = new [] { "Orc.FileAssociation" };
var company = "WildGums";
var startYear = 2010;
var defaultRepositoryUrl = string.Format("https://github.com/{0}/{1}", company, projectName);

#l "./deployment/cake/variables.cake"
#l "./deployment/cake/tasks.cake"
