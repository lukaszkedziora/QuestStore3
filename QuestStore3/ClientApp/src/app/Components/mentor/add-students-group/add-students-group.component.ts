import { Component, Inject, OnInit } from '@angular/core';
import { UsersFull } from '../../user/usersFull';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl} from '@angular/forms';




@Component({
  selector: 'app-add-students-group',
  templateUrl: './add-students-group.component.html',
  styleUrls: ['./add-students-group.component.css']
})
export class AddStudentsGroupComponent implements OnInit {
  public users: UsersFull[];
  private checkBox: FormGroup;
  private checkUser: UsersFull[];


 constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
   http.get<UsersFull[]>(baseUrl + 'api/groups/get-to-group').subscribe(result => {
      this.users = result;
   }, error => console.error(error));
 }

 initializeForm() {
    this.checkBox = new FormGroup({
      'checkboxanswer': new FormControl(false),
      //'id': new FormControl(),
    });
  }

  ngOnInit(): void {
 
  this.initializeForm();

  }

  onSubmit() {
    this.checkUser = this.checkBox.value;
  
  }
}
