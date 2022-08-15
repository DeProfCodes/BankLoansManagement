
var dataTable;

$(document).ready(function ()
{
  loadDataTable();
  LoadLoanTotals();

  $.fn.dataTable.ext.search.push(
    function (settings, searchData, index, rowData, counter)
    {
      var positions = $('input:checkbox[name="pos"]:checked').map(function ()
      {
        return this.value;
      }).get();

      if (positions.length === 0)
      {
        return true;
      }
      if (positions.indexOf(searchData[4]) !== -1)
      {
        return true;
      }
      return false;
    }
  );

  var table = $('#DT_load').DataTable();
  $('input:checkbox').on('change', function ()
  {
    console.log(table);
    table.draw();
  });
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
      { "data": "loanAmount", "width": "12%", render: $.fn.dataTable.render.number(' ', ',', 2, 'R') },
      { "data": "loanType", "width": "12%" },
      {
        "data": "loanInterestRate", "width": "12%"},/*, "render": function (data, type, row, meta) { return '' + data + '%'; }*/
      { "data": "loanTotalAmount", "width": "12%", render: $.fn.dataTable.render.number(' ', ',', 2, 'R') },
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
    "width": "100%",
    columnDefs: [
      { type: 'currency', targets: 0 }
    ]
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


function LoadLoanTotals()
{
  $.ajax({
    type: "GET",
    url: "/loans/gettotals",
    datatype: "json",
    success: function (res)
    {
      document.getElementById("totalFees").innerHTML = formatter.format(res.data.loanFeesTotal);
      document.getElementById("loanTotals").innerHTML = formatter.format(res.data.loanTotals);
    }
  });
}

var formatter = new Intl.NumberFormat('en-ZA', {
  style: 'currency',
  currency: 'ZAR',
});