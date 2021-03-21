import { HttpInterceptor, HttpRequest, HttpEvent, HttpHandler, HttpXsrfTokenExtractor } from "@angular/common/http";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";

@Injectable()
export class HttpConfigInterceptor implements HttpInterceptor {

  constructor(private xsrfTokenExtractor: HttpXsrfTokenExtractor) {
  }


  intercept(
    req: HttpRequest<any>, next: HttpHandler
  ): Observable<HttpEvent<any>> {

    //if (localStorage.getItem('cki') == null) {
    let xsrfToken = this.getCookie('XSRF-TOKEN');//this.xsrfTokenExtractor.getToken() as string;
    //  localStorage.setItem('cki', xsrfToken);
    //}

    //let xsrfToken = "teest token";
    //const loggedUser = jsonInfo ? JSON.parse(jsonInfo) : null;
    //request = request.clone({
    //  headers: request.headers.set("X-XSRF-TOKEN", "Hello")
    //});.set("Authorization", "Bearer " +

    req = req.clone({ headers: req.headers.set("Content-Type", "application/json") });
    var token = localStorage.getItem('jwttoken');

    //if (token && req.url.indexOf("authenticate") == -1) {
    //  req = req.clone({ headers: req.headers.set('Authorization', 'Bearer ' + token) });
    //}

    if (req.method == "POST" && xsrfToken != null) {
      req = req.clone({ headers: req.headers.set("XSRF-TOKEN", xsrfToken) });
    }

    //if (req.method == "POST" && xsrfToken !== null) {
    //  const authorizedRequest = req.clone({
    //    //withCredentials: true,
    //    headers: req.headers.set("XSRF-TOKEN", xsrfToken)
    //  });


    //  return next.handle(authorizedRequest);
    //} else {
      return next.handle(req);
    //}
  }

  getCookie(cname) {
  var name = cname + "=";
  var decodedCookie = decodeURIComponent(document.cookie);
  var ca = decodedCookie.split(';');
  for (var i = 0; i < ca.length; i++) {
    var c = ca[i];
    while (c.charAt(0) == ' ') {
      c = c.substring(1);
    }
    if (c.indexOf(name) == 0) {
      return c.substring(name.length, c.length);
    }
  }
  return "";
}
}

//import { Injectable } from '@angular/core';
////import { ErrorDialogService } from '../error-dialog/errordialog.service';
//import {
//  HttpInterceptor,
//  HttpRequest,
//  HttpResponse,
//  HttpHandler,
//  HttpEvent,
//  HttpErrorResponse,
//  HttpXsrfTokenExtractor
//} from '@angular/common/http';

//import { Observable, throwError } from 'rxjs';
//import { map, catchError } from 'rxjs/operators';

////@Injectable()
//export class HttpConfigInterceptor implements HttpInterceptor {
//  constructor(private xsrfTokenExtractor: HttpXsrfTokenExtractor) { }//public errorDialogService: ErrorDialogService
//  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//    // const token: string = localStorage.getItem('token');
//    let xsrfToken = this.xsrfTokenExtractor.getToken();
//    if (xsrfToken) {
//      request = request.clone({ headers: request.headers.set('Authorization', 'Bearer ' + xsrfToken) });
//    }

//    if (!request.headers.has('Content-Type')) {
//      request = request.clone({ headers: request.headers.set('Content-Type', 'application/json') });
//    }

//    request = request.clone({ headers: request.headers.set('Accept', 'application/json') });
//    request = request.clone({ headers: request.headers.set("X-XSRF-TOKEN", xsrfToken) });

//    return next.handle(request).pipe(
//     map((event: HttpEvent<any>) => {
//       if (event instanceof HttpResponse) {
//         console.log('event--->>>', event);
//         // this.errorDialogService.openDialog(event);
//       }
//       return event;
//     }),
//     catchError((error: HttpErrorResponse) => {
//       let data = {};
//       data = {
//         reason: error && error.error && error.error.reason ? error.error.reason : '',
//         status: error.status
//       };
//       //this.errorDialogService.openDialog(data);
//       return throwError(error);
//     }));
//  }
//}
