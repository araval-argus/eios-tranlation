/* eslint-disable @typescript-eslint/naming-convention */
import { Injectable } from '@angular/core';
import { from } from 'rxjs';
import { tap } from 'rxjs/operators';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor,
    HttpResponse,
    HttpHeaders,
} from '@angular/common/http';

@Injectable({
    providedIn: 'root',
})
export class Interceptor implements HttpInterceptor {
    protected defaultTimeout: number;
    constructor(
    ) { }
    intercept(
        req: HttpRequest<any>,
        next: HttpHandler
    ) {
        // convert promise to observable using 'from' operator
        return from(this.handle(req, next));
    }
    async handle(req: HttpRequest<any>,
        next: HttpHandler) {
        return next.handle(req).pipe(
            tap(async (event: HttpEvent<any>) => { },
                async (err: any) => {
                    switch (err.status) {
                        case 401:
                            console.log(11)
                            break;
                    }
                }
            )).toPromise();
    }
}
