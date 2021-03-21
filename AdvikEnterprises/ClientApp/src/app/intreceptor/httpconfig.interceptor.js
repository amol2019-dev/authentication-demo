"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
//@Injectable()
var HttpConfigInterceptor = /** @class */ (function () {
    function HttpConfigInterceptor() {
    }
    HttpConfigInterceptor.prototype.intercept = function (request, next) {
        var storageUser = localStorage.getItem('LoggedUser');
        //const loggedUser = jsonInfo ? JSON.parse(jsonInfo) : null;
        request = request.clone({
            headers: request.headers.set("X-XSRF-TOKEN", "Hello")
        });
        return next.handle(request);
    };
    return HttpConfigInterceptor;
}());
exports.HttpConfigInterceptor = HttpConfigInterceptor;
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
//# sourceMappingURL=httpconfig.interceptor.js.map