
var usersDataTable;

$(document).ready(function ()
{
  loadUsersDataTable();
});

function loadUsersDataTable(filter)
{
  usersDataTable = $('#DT_Users').DataTable({
    "ajax": {
      "url": "/users/getall/",
      "type": "GET",
      "datatype": "json"
    },
    "columns": [
      { "data": "firstName", "width": "20%" },
      { "data": "lastName", "width": "20%" },
      { "data": "idNumber", "width": "20%" },
      {
        "data": "userId",
        "render": function (data)
        {
          return `<div class="text-center">
                        <a href="/Users/Edit?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Edit
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=DeleteUser('/Users/Delete?id='+${data})>
                            Delete
                        </a>
                        &nbsp;
                        <a href="/Loans/Create?userId=${data}" class='btn btn-primary text-white' style='cursor:pointer; width:100px;'>
                            Add Loan
                        </a>
                        </div>`;
        }, "width": "40%"
      }
    ],
    "language": {
      "emptyTable": "no data found"
    },
    "width": "100%"
  });
}

function DeleteUser(url)
{
  swal({
    title: "Are you sure?",
    text: "Loans associated with this user will be deleted!",
    icon: "warning",
    buttons: true,
    dangerMode: true
  }).then((willDelete) =>
  {
    if (willDelete)
    {
      $.ajax({
        type: "DELETE",
        url: url,
        success: function (data)
        {
          if (data.success)
          {
            toastr.success(data.message);
            usersDataTable.ajax.reload();
          }
          else
          {
            toastr.error(data.message);
          }
        }
      });
    }
  });
}