namespace Mats.Edvardsen.TestingStuff.Web.Tests;

public class CopyProjectFeatureTests
{
//WIKI: https://dev.azure.com/bdodev/BIP/_wiki/wikis/BIP%20Docs/3520/Test-cases
    [Fact]
    public async Task Feature_CopyProject_WithValid_Data()
    {
        
// In Salesforce: Find a project with a "revisjonsprosess" or "flow fremdriftsplan"
// that can be continued. Make sure that the project exists in WD as well.
        var prevProjectId = SfClient.SetupProjectTemplateData(previousYear: 2023);
        var oldProjectWDId = WDClient.SetupWorkdayData(prevProjectId);
        var workdayProjectId = SfClient.CreateNewProjectFromPeviousYear(prevProjectId);
        
        
//Click the account under "Kontonavn" within the project.
// Go to "Relatert", scroll down to "Prosjekt" and click "Ny"/"Opprett".
        
        // figure out how to programatically click the "Ny"/"Opprett" from previous project button
        SfClient.PostCreateNewFromPreviousYearJob();
        
        // Verify in the function-app log that the following logs occured without exceptions:
        // 7.1 "Starting function with executionId: {ExecutionId}", executionId
        // 7.2 "Received message from topic: {Message}"
        // 7.3 "Completed execution of CopyProject function with executionId: {ExecutionId}"
        OrchestratorClient.VerifyJobStatus(new {Topic="X",JobStatus="Completed"});
        
        // Verify changes in WD:
        // 8.1 Verify that new project is created with correct values in WD
        var createdProject = WDClient.GetProject(workdayProjectId).Should().NotBeNull();
        // 8.2 Verify that roles have been copied from old to new project with correct values in WD
        var oldRoles = WDClient.GetRolesForProject(oldProjectWDId);
        var newRoles = WDClient.GetRolesForProject(workdayProjectId);
        AssertionHelper.AssertRolesCreatedAsExpected(oldRoles, newRoles);
        // 8.3 Verify that resourcePlaner have been copied from old to new project with correct values in WD
        // 8.4 Verify that resourceForcast have been copied from old to new project with correct values in WD (Should not copy on Audit projects)
        // 8.5 Verify that projectPlanWithTemplate have been copied from old to new project with correct values in WD
        // 8.6 Verify that projectPlan have copied from old to new project with correct values in WD ( Its very important to validate the ratesheet amount here, it should be adjusted by XX.XX amount percent from last years value)

        // Check that the project in salesforce is updated with a project ID.
        //
        //Check that the billing schedule has been copied with fields (PO number, Invoice Type, Payment Terms)
        //
    }
}

public class AssertionHelper
{
    public static void AssertRolesCreatedAsExpected(object oldRoles, object newRoles)
    {
        throw new NotImplementedException();
    }
}

public class OrchestratorClient
{
    public static void VerifyJobStatus(object o)
    {
        throw new NotImplementedException();
    }
}

public class WDClient
{
    public static object SetupWorkdayData(object prevProjectId)
    {
        throw new NotImplementedException();
    }

    public static object GetProject(object projectId)
    {
        throw new NotImplementedException();
    }

    public static object GetRolesForProject(object projectId)
    {
        throw new NotImplementedException();
    }
}

public class SfClient
{
    public static object CreateProjectByTestTemplate(int previousYear) => throw new NotImplementedException();

    public static object CreateNewProjectFromPeviousYear(object prevProjectId)
    {
        throw new NotImplementedException();
    }

    public static void PostCreateNewFromPreviousYearJob()
    {
        throw new NotImplementedException();
    }

    public static object SetupProjectTemplateData(int previousYear)
    {
        throw new NotImplementedException();
    }
}

public class HttpClient
{
   public static async Task PostToApiManagement(string request)
    {
        
    }
}

public static class TestData
{
    public static Task Setup(string tag)
    {
        return Task.CompletedTask;
    }
}