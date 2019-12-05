function get_cookie(cookie_name) {
 
    let results = document.cookie.match('(^|;) ?' + cookie_name + '=([^;]*)(;|$)');
  //  console.log(unescape(results[2]) + " 2: " + decodeURI(results[2]) + " 1: " + results[1]+" 0: "+results[0]);
    if (results)
        return (decodeURI(results[2]));
    else
        return null;
}

function set_cookie(cookie_name, cookie_val, expireSec) {

    let cookie_date = new Date();
    cookie_date.setSeconds(cookie_date.getSeconds() + expireSec);
    document.cookie = cookie_name + "=" + cookie_val + "; expires=" + cookie_date.toGMTString();
  //  return "";
}


var flFirstTab = true;

if (get_cookie("logged_in") == null || get_cookie("logged_in") == 'no')
{   //   document.cookie = "logged_in=yes;";
    set_cookie("logged_in", "yes", 7200);
}
else {
    window.location.href = "/Login/ErrorNewTab";
}


window.addEventListener('beforeunload', function (e) {
    if (flFirstTab) { set_cookie("logged_in", "no", -20);  }
}, false);


