import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { Exposure } from './exposure';
import { Position } from './position';

@Injectable()
export class ExposureService {
  private baseUrl: string = 'http://localhost:3374/api';

  constructor(private http: Http) {
  }

  getAll(): Observable<Exposure[]> {
    let exposures$ = this.http
      .get(`${this.baseUrl}/ViewPosition`, { headers: this.getHeaders() })
      .map((res: Response) => <Exposure[]>res.json())
      .map(toExposure)
      .catch(handleError);
    return exposures$;
  }

  getAllInterval(): Observable<Exposure[]> {
    let fetchExposures$ =
      this.http
        .get(`${this.baseUrl}/ViewPosition`, { headers: this.getHeaders() })
        .map(mapExposures);
    return Observable
      .interval(1000)
      .switchMap((index)=>fetchExposures$)
      .catch(handleError);
  }

  get(id: number): Observable<Exposure> {
    let exposure$ = this.http
      .get(`${this.baseUrl}/exposure/${id}`, { headers: this.getHeaders() })
      .map(mapExposure);
    return exposure$;
  }

  save(exposure: Exposure): Observable<Response> {
    return this.http
      .put(`${this.baseUrl}/exposure/${exposure.key}`, JSON.stringify(exposure), { headers: this.getHeaders() });
  }

  private getHeaders() {
    let headers = new Headers();
    headers.append('Accept', 'application/json');
    return headers;
  }
}

function fetchExposures(): Observable<Exposure[]> {
  return this.http
    .get(`${this.baseUrl}/ViewPosition`, { headers: this.getHeaders() })
    .map((res: Response) => <Exposure[]>res.json())
    .catch(handleError);
}

function toExposure(r: any): Exposure {
  let exposure = <Exposure>r;
  console.log('Parsed exposure:', exposure);
  return exposure;
}

function mapExposure(response: Response): Exposure {
  return toExposure(response.json());
}

function mapExposures(response: Response): Exposure[] {
  return <Exposure[]>response.json();
}

// this could also be a private method of the component class
function handleError(error: any) {
  // log error
  // could be something more sofisticated
  let errorMsg = error.message || `Yikes! We couldnt retrieve your data!`
  console.error(errorMsg);

  // throw an application level error
  return Observable.throw(errorMsg);
}