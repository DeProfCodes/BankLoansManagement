
var dataTable;

$(document).ready(function ()
{
  loadDataTable();
});

function loadDataTable()
{
  dataTable = $('#DT_load').DataTable({
    "ajax": {
      "url": "/loans/getall/",
      "type": "GET",
      "datatype": "json"
    },
    "columns": [
      { "data": "clientFirstName", "width": "12%" },
      { "data": "clientLastName", "width": "12%" },
      { "data": "clientIdNumber", "width": "12%" },
      { "data": "loanAmount", "width": "12%" },
      { "data": "loanType", "width": "12%" },
      { "data": "loanInterestRate", "width": "12%" },
      { "data": "loanTotalAmount", "width": "12%" },
      {
        "data": "loanId",
        "render": function (data)
        {
          return `<div class="text-center">
                        <a href="/Loans/Edit?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Edit
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=DeleteLoan('/Loans/Delete?id='+${data})>
                            Delete
                        </a>
                        </div>`;
        }, "width": "12%"
      }
    ],
    "language": {
      "emptyTable": "no data found"
    },
    "width": "100%"
  });
}

function DeleteLoan(url)
{
  swal({
    title: "Are you sure?",
    text: "Once loan is deleted, you will not be able to recover",
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
            dataTable.ajax.reload();
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