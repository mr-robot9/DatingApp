import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() registerCancel = new EventEmitter();
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  register(){
    this.authService.register(this.model).subscribe( next => {
      console.log('Registered successfully');
      this.cancel();
    },
    error => {
      console.log(error);
    });;

  }

  cancel(){
    
    this.registerCancel.emit(false);
    console.log("cancel");
  }

}
