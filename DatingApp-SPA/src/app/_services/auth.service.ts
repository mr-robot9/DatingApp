import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map, catchError} from 'rxjs/operators';
import { throwError, Observable, empty, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = 'http://localhost:5000/api/auth/';
constructor(private http: HttpClient) { }

login(model: any) {
  console.log(model);
  return this.http.post(this.baseUrl + 'login', model)
  .pipe(
    map((response: any) => {
      const tokenObj = response;
      if (tokenObj) { // if not null
        localStorage.setItem('token', tokenObj.token);
      }
    }),
    catchError((error) => {
      console.log(error.error);
      return throwError(error);
    })
  );
}

register(model: any){
  console.log(model);
  return this.http.post(this.baseUrl +'register', model);
}

handleError(error: Response) {

    return throwError(error);
}

}
