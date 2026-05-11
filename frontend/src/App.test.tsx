import { render, screen, waitFor } from '@testing-library/react';
import App from './App';
import { libraryApi } from './api/libraryApi';

jest.mock('./api/libraryApi', () => ({
  libraryApi: {
    getBooks: jest.fn(),
    getBook: jest.fn(),
    createBook: jest.fn(),
    updateBook: jest.fn(),
    deleteBook: jest.fn(),
    toggleAvailability: jest.fn(),
  },
}));

const mockBooks = [
  {
    id: 1,
    title: 'The Great Gatsby',
    author: 'F. Scott Fitzgerald',
    isbn: '978-0743273565',
    publishedDate: '1925-04-10',
    owner: 'Alice',
    isAvailable: true,
  },
  {
    id: 2,
    title: 'To Kill a Mockingbird',
    author: 'Harper Lee',
    isbn: '978-0061935466',
    publishedDate: '1960-07-11',
    owner: 'Bob',
    isAvailable: false,
  },
];

describe('App', () => {
  beforeEach(() => {
    (libraryApi.getBooks as jest.Mock).mockResolvedValue([]);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders page heading and subtitle', () => {
    render(<App />);
    expect(screen.getByText('Library')).toBeInTheDocument();
    expect(screen.getByText('Crumb-to-Crumb Book Lending')).toBeInTheDocument();
  });

  it('shows loading state while fetching', () => {
    (libraryApi.getBooks as jest.Mock).mockReturnValue(new Promise(() => {}));
    render(<App />);
    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });

  it('renders books returned by the API', async () => {
    (libraryApi.getBooks as jest.Mock).mockResolvedValue(mockBooks);
    render(<App />);
    await waitFor(() => expect(screen.getByText('The Great Gatsby')).toBeInTheDocument());
    expect(screen.getByText('To Kill a Mockingbird')).toBeInTheDocument();
  });

  it('shows book count after fetch', async () => {
    (libraryApi.getBooks as jest.Mock).mockResolvedValue(mockBooks);
    render(<App />);
    await waitFor(() => expect(screen.getByText('2 books')).toBeInTheDocument());
  });

  it('shows error message when fetch fails', async () => {
    (libraryApi.getBooks as jest.Mock).mockRejectedValue(new Error('Network error'));
    render(<App />);
    await waitFor(() =>
      expect(screen.getByText('Failed to load books. Is the API running?')).toBeInTheDocument()
    );
  });

  it('shows Add Book button', () => {
    render(<App />);
    expect(screen.getByText('+ Add Book')).toBeInTheDocument();
  });
});
