using CommandLine.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Performance.Application.Interface.Repository;
using Performance.Application.Interface.UnitOfWork;
using Performance.Domain.Entity;

namespace Performance.API.Controllers
{
    public class UsersODataController (IUnitOfWork _unitOfWork)
        : ODataController
    {

        [HttpGet]
        [EnableQuery(PageSize = 1000, MaxExpansionDepth = 3)]
        public IActionResult Get()
        {
            return Ok(_unitOfWork.UserRepository.GetAll());
        }
    }
}


//# Get all users
//"https://localhost:7178/odata/UsersOData"

//# Select specific fields only
//"https://localhost:7178/odata/UsersOData?$select=firstName,lastName,email"

//# Filter by id
//"https://localhost:7178/odata/UsersOData?$filter=Id%20eq%201"

//# Filter by email
//"https://localhost:7178/odata/UsersOData?$filter=Email%20eq%20'user1@example.com'"

//# Filter by role
//"https://localhost:7178/odata/UsersOData?$filter=Roles/any(r:r/Name%20eq%20'User')"

//# Filter by id and include 1 child
//"https://localhost:7178/odata/UsersOData?$filter=Id%20eq%201&$expand=Roles"

//# Filter by id and include multiple childs
//"https://localhost:7178/odata/UsersOData?$filter=Id%20eq%201&$expand=Roles,Address,BankAccount($expand=Transactions)"

//# Offset pagination  
//"https://localhost:7178/odata/UsersOData?$top=10&$skip=0&$orderby=Id%20asc&$count=true"

//# Cursor pagination 
//"https://localhost:7178/odata/UsersOData?$top=10&$filter=Id%20gt%2010&$orderby=Id%20asc&$count=true"