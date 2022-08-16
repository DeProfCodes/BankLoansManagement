
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

var scrolled = 0;

function myScroll()
{
  console.log("WE HERE!");
  $(document).scroll($(document).height());
}