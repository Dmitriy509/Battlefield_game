function get_cookie(cookie_name) {
 
    let results = document.cookie.match('(^|;) ?' + cookie_name + '=([^;]*)(;|$)');
  //  console.log(unescape(results[2]) + " 2: " + decodeURI(results[2]) + " 1: " + results[1]+" 0: "+results[0]);
    if (results)
        return (decodeURI(results[2]));
    else
        return null;
}
