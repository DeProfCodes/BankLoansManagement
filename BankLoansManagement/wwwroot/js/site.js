
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

function GenerateRandomUsers()
{
  swal({
    title: "Are you sure?",
    text: "This will create 10 new users",
    icon: "warning",
    buttons: true,
    dangerMode: true
  }).then((willCreate) =>
  {
    if (willCreate)
    {
      $.ajax({
        type: "POST",
        url: "/users/CreateRandom/",
        error: function (xhr, ajaxOptions, thrownError)
        {
          swal({ title: 'Failed', text: "Something went wrong, try again!", icon: 'error' });
        },
        success: function (data)
        {
          swal({ title: 'Complete', text: "Users generated", icon: 'success' });
          dataTable.ajax.reload();
        }
      });
    }
  });
}