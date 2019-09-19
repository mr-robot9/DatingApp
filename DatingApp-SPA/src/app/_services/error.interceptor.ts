import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor{
    intercept(
        req: import('@angular/common/http').HttpRequest<any>, 
        next: import('@angular/common/http').HttpHandler): import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(error => {
                if(error === 401){
                    return throwError(error.statusText);
                }

                //handle model state errors
                if (error instanceof HttpErrorResponse){
                    const serverError = error.error;
                    let modelStateErrors = '';
    
                    //if it has the errors array containing an objects
                    if(serverError && typeof serverError === 'object'){
                        //foreach error, add to string
                        for (const key in serverError){
                            if (serverError[key]){
                                modelStateErrors += serverError[key] + '\n';
                            }
                        }
                    }
       
                    return throwError(modelStateErrors || 'Server Error');

                }
            })
        )
    }
}

//we want to configure the interceptor, this is not used by classes we've created
export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
};