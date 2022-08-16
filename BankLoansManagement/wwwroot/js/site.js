
function ToggleMenu()
{
  var x = document.getElementById("myTopnav");
  if (x.className === "topnav")
  {
    x.className += " responsive";
  } else
  {
    x.className = "topnav";
  }
}

$("#loaderDiv").hide();

function GenerateRandomUsers()
{
  swal({
    title: "Are you sure?",
    text: "This will create 10 new users",
    icon: "warning",
    buttons: true,
    dangerMode: false
  }).then((willCreate) =>
  {
    if (willCreate)
    {
      $.ajax({
        type: "POST",
        url: "/users/CreateRandom/",
        beforeSend: function ()
        {
          $("#loaderDiv").show()
        },
        error: function (xhr, ajaxOptions, thrownError)
        {
          swal({ title: 'Failed', text: "Something went wrong, try again!", icon: 'error' });
        },
        success: function (data)
        {
          $("#loaderDiv").hide();
          swal({ title: 'Complete', text: "Users generated", icon: 'success' });
          usersDataTable.ajax.reload();
        }
      });
    }
  });
}

function DeleteAllUsers()
{
  swal({
    title: "Are you sure?",
    text: "This will all Users and all Loans",
    icon: "warning",
    buttons: true,
    dangerMode: true
  }).then((willDelete) =>
  {
    if (willDelete)
    {
      $.ajax({
        type: "POST",
        url: "/users/DeleteAllUsers/",
        beforeSend: function ()
        {
          $("#loaderDiv").show()
        },
        error: function (xhr, ajaxOptions, thrownError)
        {
          swal({ title: 'Failed', text: "Something went wrong, try again!", icon: 'error' });
        },
        success: function (data)
        {
          $("#loaderDiv").hide();
          swal({ title: 'Complete', text: "All Users and all Loans have been deleted successfully", icon: 'success' });
          usersDataTable.ajax.reload();
        }
      });
    }
  });
}

function GenerateTenLoansForRandomUsers()
{
  swal({
    title: "Are you sure?",
    text: "This will create 10 loans for random users",
    icon: "warning",
    buttons: true,
    dangerMode: false
  }).then((willCreate) =>
  {
    if (willCreate)
    {
      $.ajax({
        type: "POST",
        url: "/loans/CreateTenLoans/",
        beforeSend: function ()
        {
          $("#loaderDiv").show()
        },
        error: function (xhr, ajaxOptions, thrownError)
        {
          swal({ title: 'Failed', text: "Something went wrong, try again!", icon: 'error' });
        },
        success: function (data)
        {
          $("#loaderDiv").hide();
          swal({ title: 'Complete', text: "New loans generated!", icon: 'success' });
          dataTable.ajax.reload();
        }
      });
    }
  });
}

function DeleteAllLoans()
{
  swal({
    title: "Are you sure?",
    text: "This will delete all Loans",
    icon: "warning",
    buttons: true,
    dangerMode: true
  }).then((willDelete) =>
  {
    if (willDelete)
    {
      $.ajax({
        type: "POST",
        url: "/loans/DeleteAllLoans/",
        beforeSend: function ()
        {
          $("#loaderDiv").show()
          window.setTimeout($("#loaderDiv").show(), 5000);
        },
        error: function (xhr, ajaxOptions, thrownError)
        {
          swal({ title: 'Failed', text: "Something went wrong, try again!", icon: 'error' });
        },
        success: function (data)
        {
          $("#loaderDiv").hide();
          swal({ title: 'Complete', text: "All Loans have been deleted successfully", icon: 'success' });
          dataTable.ajax.reload();
        }
      });
    }
  });
}