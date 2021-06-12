import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '@app/shared/models';
import { UsersService } from '@app/shared/services';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
    public users$: Observable<User[]>;

    constructor(
        private usersService: UsersService
    ) { }

  ngOnInit(): void {
      this.users$ = this.usersService.getAll();
  }

}
