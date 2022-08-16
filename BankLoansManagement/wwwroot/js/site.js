
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
          window.setTimeout($("#loaderDiv").show(), 5000);
        },
        error: function (xhr, ajaxOptions, thrownError)
        {
          swal({ title: 'Failed', text: "Something went wrong, try again!", icon: 'error' });
        },
        success: function (data)
        {
          $("#loaderDiv").hide();
          swal({ title: 'Complete', text: "Users generated", icon: 'success' });
          dataTable.ajax.reload();
        }
      });
    }
  });
}