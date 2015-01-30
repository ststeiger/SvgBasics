
$.getJSON = function (u, d, s)
{
    if (typeof (d) == "function")
    { s = d; d = null; }

    return $.ajax({
        dataType: "json"
           , url: u
           , data: d
           , success: s
           , error: function (err)
           {
               console.log("error $.getJSON");
               console.log(err);
           }
    });
};
////////////////
$.get = function (u, d, s, t)
{
    if (typeof (d) == "function")
    { t = s; s = d; d = null; }

    return $.ajax({
        url: u
      , data: d
      , success: s
      , error: function (err)
      {
          console.log("error $.get");
          console.log(err);
      }
      , dataType: t
    });
};
/////////
$.post = function (u, d, s, t)
{
    if (typeof (d) == "function")
    { t = s; s = d; d = null; }

    return $.ajax({
        type: "POST"
      , url: u
      , data: d
      , success: s
      , error: function (err)
      {
          console.log("error $.post");
          console.log(err);
      }
      , dataType: t
    });
};
// 

$.jsonp = function (u, params, cb, cbn, fn)
{
    var sep = "?", ps = "";
    if (params != null)
    {
        if (u !== null && u.indexOf("?") !== -1) sep = "&";
        for (var key in params)
        {
            // important check that this is objects own property
            // not from prototype prop inherited
            if (params.hasOwnProperty(key))
            {
                ps += sep + encodeURIComponent(key) + "=" + encodeURIComponent(params[key]);
                sep = "&";
            }
        } // Next key
    } // End if(params != null)

    return $.ajax({
        url: (u + ps)
      , type: 'jsonp'
      , jsonpCallback: cb
      , jsonpCallbackName: cbn
     , success: fn
        //,error : function(err) { console.log("error $.jsonp");console.log(err);}
    })
};

