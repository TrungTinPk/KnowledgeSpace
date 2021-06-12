import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { User } from '../models/user.model';
import { environment } from '@environments/environment';
import { UtilitiesService } from '@app/shared/services/utilities.service';

@Injectable({ providedIn: 'root' })
export class UsersService extends BaseService {
    constructor(
        private http: HttpClient,
        private utilitiesService: UtilitiesService
    ) {
        super();
    }
    getAll() {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
        return this.http.get<User[]>(`${environment.apiUrl}/api/users`, httpOptions)
            .pipe(catchError(this.handleError));
    }

    getMenuByUser (userId: string) {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
        return this.http.get<Function[]>(`${environment.apiUrl}/api/users/${userId}/menu`, httpOptions)
            .pipe(map(response => {
                const functions = this.utilitiesService.unflatteringForLeftMenu(response);
                return functions;
            }), catchError(this.handleError));
    }
}
